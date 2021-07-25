﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectionUtils;

// TODO: If we have extra time, refactor the code of this module to be more DRY.

public class TilePuzzle : MonoBehaviour
{
    [SerializeField] private int inactiveIndex = 3;
    [SerializeField] private int xCount = 3;
    [SerializeField] private int yCount = 3;
    [SerializeField] private float xPadding = 2f;
    [SerializeField] private float yPadding = 2f;
    [SerializeField] private float tileSize = 10f;
    [SerializeField] private bool isDone = false;
    [SerializeField] private Vector2 center = new Vector2(0, 0);

    /**
     * Make sure that the sprites applied to this field follow this order:
     *
     * Example 3x3:
     * ----------------------------------------------------
     * | Sprite Index 6 | Sprite Index 7 | Sprite Index 8 |
     * ----------------------------------------------------
     * | Sprite Index 3 | Sprite Index 4 | Sprite Index 5 |
     * ----------------------------------------------------
     * | Sprite Index 0 | Sprite Index 1 | Sprite Index 2 |
     * ----------------------------------------------------
     *
     * Example 3x4:
     * -------------------------------------------------------------------------
     * | Sprite Index 8  | Sprite Index 9  | Sprite Index 10 | Sprite Index 11 |
     * -------------------------------------------------------------------------
     * | Sprite Index 4  | Sprite Index 5  | Sprite Index 6  | Sprite Index 7  |
     * -------------------------------------------------------------------------
     * | Sprite Index 0  | Sprite Index 1  | Sprite Index 2  | Sprite Index 3  |
     * -------------------------------------------------------------------------
     */
    [SerializeField] private Sprite[] tileSprite;

    private string tileNamePrefix = "Tile";
    private string snapperNamePrefix = "SnapDetector";
    private int currentEmptyIndex;
    private List<Vector2> snappingPoints = new List<Vector2>();
    private List<GameObject> snappingDetectors = new List<GameObject>();
    private List<GameObject> tiles = new List<GameObject>();
    private List<GameObject> hiddenTiles = new List<GameObject>();
    private Plane draggingPlane;
    private Vector3 offset;
    private Camera mainCamera;
    private GameObject selectedTile;
    private Dictionary<int, Vector2> currentConnections = new Dictionary<int, Vector2>();

    private GameObject CreateTile(int tileId, int xOffset, int yOffset)
    {
        GameObject tile = new GameObject();
        tile.transform.localScale = new Vector3(this.tileSize, this.tileSize, this.tileSize);
        tile.name = $"{this.tileNamePrefix}-{tileId}";
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();

        renderer.sprite = this.tileSprite[tileId];
        Tile tileScript = tile.AddComponent<Tile>();
        tileScript.correctIndex = tileId;
        tileScript.currentIndex = tileId;
        BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        return tile;
    }

    private GameObject CreateSnappingDetector(int snapId)
    {
        GameObject snappingDetector = new GameObject();
        snappingDetector.name = $"{this.snapperNamePrefix}-{snapId}";
        snappingDetector.AddComponent<BoxCollider2D>();
        SnappingDetector snapComponent = snappingDetector.AddComponent<SnappingDetector>();
        snapComponent.snapIndex = snapId;
        return snappingDetector;
    }

    private void InitializeTiles()
    {
        float absoluteXCount = Math.Abs(xCount);
        float floatedXCount = (float)absoluteXCount;
        float unpaddedMaxX = (floatedXCount - 1f) * xPadding * 0.5f;
        float unpaddedMinX = -unpaddedMaxX;

        float absoluteYCount = Math.Abs(yCount);
        float floatedYCount = (float)absoluteYCount;
        float unpaddedMaxY = (floatedYCount - 1f) * yPadding * 0.5f;
        float unpaddedMinY = -unpaddedMaxY;

        int tileId = 0;
        int yOffset = 0;
        for(float i = unpaddedMinY; i <= unpaddedMaxY; i += yPadding, yOffset++)
        {
            float y = i + this.center.y;
            int xOffset = 0;
            for(float j = unpaddedMinX ; j <= unpaddedMaxX; j += xPadding, xOffset++)
            {
                float x = j + this.center.x;
                Vector2 position = new Vector2(x, y);
                GameObject snapDetector = this.CreateSnappingDetector(tileId);
                GameObject tile = this.CreateTile(tileId, xOffset, yOffset);
                Tile tileComponent = tile.GetComponent<Tile>();
                SnappingDetector snapComponent = snapDetector.GetComponent<SnappingDetector>();
                snapDetector.transform.position = new Vector3(
                    position.x,
                    position.y,
                    1f
                );
                tileComponent.autoTarget = position;
                tile.transform.position = position;
                snapComponent.occupied = true;
                if(tileId == this.inactiveIndex)
                {
                    snapComponent.occupied = false;
                    this.hiddenTiles.Add(tile);
                    tile.SetActive(false);
                }
                this.snappingDetectors.Add(snapDetector);
                this.snappingPoints.Add(position);
                this.tiles.Add(tile);
                tileId++;
            }
        }
    }

    private void ShuffleTiles()
    {
        List<int> indices = new List<int>();
        List<int> shuffledIndices = new List<int>();

        for(int i = 0; i < this.tiles.Count; i++)
        {
            int index = this.tiles[i].GetComponent<Tile>().correctIndex;
            indices.Add(index);
            shuffledIndices.Add(index);
        }

        for(int i = 0; i < indices.Count; i++)
        {
            int originalIndex = indices[i];
            int shuffledIndex = originalIndex;
            do
            {
                shuffledIndex = UnityEngine.Random.Range(0, indices.Count - 1);
            }
            while(shuffledIndex == originalIndex);
            int tempIndex = shuffledIndices[originalIndex];
            shuffledIndices[originalIndex] = shuffledIndices[shuffledIndex];
            shuffledIndices[shuffledIndex] = tempIndex;
        }

        for(int i = 0; i < this.tiles.Count; i++)
        {
            int newIndex = shuffledIndices[i];
            GameObject snapDetector = this.snappingDetectors[newIndex];
            SnappingDetector snapComponent = snapDetector.GetComponent<SnappingDetector>();
            snapComponent.occupied = false;
            if(this.tiles[i].activeSelf)
            {
                snapComponent.occupied = true;

                GameObject tile = this.tiles[i];
                Tile tileComponent = tile.GetComponent<Tile>();
                Vector2 position = this.snappingPoints[newIndex];

                tileComponent.currentIndex = newIndex;
                tileComponent.autoTarget = position;
                tile.transform.position = position;
            }
        }
    }

    private void StartTileDrag(GameObject tile)
    {
        Tile tileComponent = tile.GetComponent<Tile>();
        tileComponent.selected = true;

        this.draggingPlane = new Plane(this.mainCamera.transform.forward, tile.transform.position);
        Ray camRay = this.mainCamera.ScreenPointToRay(Input.mousePosition);
        float planeDistance;
        this.draggingPlane.Raycast(camRay, out planeDistance);
        this.offset = tile.transform.position - camRay.GetPoint(planeDistance);
    }

    private void UpdateTileDrag(GameObject tile)
    {
        float xRayDistance = this.xPadding;
        float yRayDistance = this.yPadding;
        RaycastHit2D[][] hitGroups = new RaycastHit2D[][] {
            Physics2D.RaycastAll(tile.transform.position, Vector2.left, xRayDistance),
            Physics2D.RaycastAll(tile.transform.position, Vector2.up, yRayDistance),
            Physics2D.RaycastAll(tile.transform.position, Vector2.right, xRayDistance),
            Physics2D.RaycastAll(tile.transform.position, Vector2.down, yRayDistance)
        };

        // reset current connections
        this.currentConnections.Clear();

        List<float> xPositions = new List<float>() { tile.transform.position.x };
        List<float> yPositions = new List<float>() { tile.transform.position.y };
        foreach(RaycastHit2D[] hits in hitGroups)
        {
            foreach(RaycastHit2D hit in hits)
            {
                if(!hit)
                    continue;

                if(!hit.collider.gameObject.name.Contains(this.snapperNamePrefix))
                    continue;

                Tile tileComponent = tile.GetComponent<Tile>();
                SnappingDetector snapComponent = hit.collider.gameObject.GetComponent<SnappingDetector>();

                if(!(snapComponent.snapIndex != tileComponent.currentIndex && !snapComponent.occupied))
                    continue;

                if(this.currentConnections.ContainsKey(snapComponent.snapIndex))
                    continue;

                Vector2 position = hit.collider.transform.position;
                xPositions.Add(position.x);
                yPositions.Add(position.y);
                this.currentConnections.Add(snapComponent.snapIndex, position);
                break;
            }
        }

        // movement
        Ray camRay = this.mainCamera.ScreenPointToRay(Input.mousePosition);
        float planeDistance;
        this.draggingPlane.Raycast(camRay, out planeDistance);
        Vector3 rawPosition = camRay.GetPoint(planeDistance) + this.offset;

        float xMin = xPositions.Min();
        float yMin = yPositions.Min();
        float xMax = xPositions.Max();
        float yMax = yPositions.Max();

        Vector3 modifiedPostion = new Vector3(
            Mathf.Clamp(rawPosition.x, xMin, xMax),
            Mathf.Clamp(rawPosition.y, yMin, yMax),
            rawPosition.z
        );
        tile.transform.position = modifiedPostion;
    }

    private void EndTileDrag(GameObject tile)
    {
        this.selectedTile = null;
        Tile tileComponent = tile.GetComponent<Tile>();
        bool isFirstIteration = true;
        float currentRecord = 0f;
        int currentIndex = 0;
        foreach(KeyValuePair<int, Vector2> connection in this.currentConnections)
        {
            float distance = Mathf.Abs(Vector2.Distance(tile.transform.position, connection.Value));
            if(isFirstIteration || currentRecord > distance)
            {
                currentRecord = distance;
                currentIndex = connection.Key;
                isFirstIteration = false;
            }
        }

        GameObject previousSnapDetector = this.snappingDetectors[tileComponent.currentIndex];
        GameObject nextSnapDetector = this.snappingDetectors[currentIndex];
        SnappingDetector previousSnapComponent = previousSnapDetector.GetComponent<SnappingDetector>();
        SnappingDetector nextSnapComponent = nextSnapDetector.GetComponent<SnappingDetector>();

        if(previousSnapComponent.occupied && nextSnapComponent.occupied)
            return;

        previousSnapComponent.occupied = false;
        nextSnapComponent.occupied = true;

        tileComponent.currentIndex = currentIndex;
        tileComponent.autoTarget = this.snappingPoints[currentIndex];
        tileComponent.selected = false;

        this.isDone = this.CheckWin();
        if(this.isDone)
            this.HandleWin();
    }

    private void HandlePuzzleInputs(GameObject tile)
    {
        if(Input.GetKeyDown(KeyMaps.TILE_SELECTOR))
        {
            StartTileDrag(tile);
        }
        if(Input.GetKey(KeyMaps.TILE_SELECTOR))
        {
            UpdateTileDrag(tile);
        }
        if(Input.GetKeyUp(KeyMaps.TILE_SELECTOR))
        {
            EndTileDrag(tile);
        }
    }

    private void HandleWin()
    {
        foreach(GameObject tile in this.hiddenTiles)
        {
            tile.SetActive(true);
        }
    }

    private bool CheckWin()
    {
        foreach(GameObject tile in this.tiles)
        {
            Tile tileComponennt = tile.GetComponent<Tile>();
            if(!tileComponennt.isPlacedCorrectly)
                return false;
        }
        return true;
    }

    void Start()
    {
        this.currentEmptyIndex = this.inactiveIndex;

        this.InitializeTiles();
        this.ShuffleTiles();
        this.mainCamera = Camera.main;
    }

    void Update()
    {
        if(this.isDone)
            return;

        ObjectSelector2D.CheckForObjectDetection(hit => {
            GameObject tile = hit.collider.gameObject;
            if(tile.name.Contains(this.tileNamePrefix))
            {
                this.selectedTile = tile;
            }
        }, 50f);

        if(this.selectedTile)
        {
            HandlePuzzleInputs(this.selectedTile);
        }
    }
}
