using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectionUtils;

public class TilePuzzle : MonoBehaviour
{
    [SerializeField] private int inactiveIndex = 3;
    [SerializeField] private int xCount = 3;
    [SerializeField] private int yCount = 3;
    [SerializeField] private float xPadding = 2f;
    [SerializeField] private float yPadding = 2f;
    [SerializeField] private float tileSize = 10f;
    [SerializeField] private Vector2 center = new Vector2(0, 0);
    [SerializeField] private Sprite tileSprite;

    private string tileNamePrefix = "Tile";
    private int currentEmptyIndex;
    private List<Vector2> snappingPoints;
    private List<GameObject> tiles;
    private Plane draggingPlane;
    private Vector3 offset;
    private Camera mainCamera;
    private GameObject selectedTile;

    private GameObject CreateTile(int tileId)
    {
        GameObject tile = new GameObject();
        tile.name = $"{this.tileNamePrefix}-{tileId}";
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = this.tileSprite;
        Tile tileScript = tile.AddComponent<Tile>();
        tileScript.correctIndex = tileId;
        tileScript.currentIndex = tileId;
        BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        return tile;
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
        for(float i = unpaddedMinY; i <= unpaddedMaxY; i += yPadding)
        {
            float y = i + this.center.y;
            for(float j = unpaddedMinX; j <= unpaddedMaxX; j += xPadding)
            {
                float x = j + this.center.x;
                Vector2 position = new Vector2(x, y);
                GameObject tile = this.CreateTile(tileId);
                tile.transform.position = position;
                if(tileId == this.inactiveIndex)
                {
                    tile.SetActive(false);
                }
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
            if(this.tiles[i].activeSelf)
            {
                int newIndex = shuffledIndices[i];
                GameObject tile = this.tiles[i];
                tile.GetComponent<Tile>().currentIndex = newIndex;
                tile.transform.position = this.snappingPoints[newIndex];
            }
        }
    }

    private void StartTileDrag(GameObject tile)
    {
        this.draggingPlane = new Plane(this.mainCamera.transform.forward, tile.transform.position);
        Ray camRay = this.mainCamera.ScreenPointToRay(Input.mousePosition);
        float planeDistance;
        this.draggingPlane.Raycast(camRay, out planeDistance);
        this.offset = tile.transform.position - camRay.GetPoint(planeDistance);
    }

    private void UpdateTileDrag(GameObject tile)
    {
        Ray camRay = this.mainCamera.ScreenPointToRay(Input.mousePosition);
        float planeDistance;
        this.draggingPlane.Raycast(camRay, out planeDistance);
        tile.transform.position = camRay.GetPoint(planeDistance) + offset;
    }

    private void EndTileDrag(GameObject tile)
    {
        this.selectedTile = null;
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

    void Start()
    {
        this.currentEmptyIndex = this.inactiveIndex;
        this.snappingPoints = new List<Vector2>();
        this.tiles = new List<GameObject>();

        this.InitializeTiles();
        this.ShuffleTiles();
        this.mainCamera = Camera.main;
    }

    void Update()
    {
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
