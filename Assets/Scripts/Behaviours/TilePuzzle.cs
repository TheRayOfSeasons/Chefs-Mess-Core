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

    private int currentEmptyIndex;
    private List<Vector2> snappingPoints;
    private List<GameObject> tiles;

    private GameObject CreateTile(int tileId)
    {
        GameObject tile = new GameObject();
        tile.name = $"Tile-{tileId}";
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = this.tileSprite;
        Tile tileScript = tile.AddComponent<Tile>();
        tileScript.index = tileId;
        BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        return tile;
    }

    void InitializeTiles()
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

    void Start()
    {
        this.currentEmptyIndex = this.inactiveIndex;
        this.snappingPoints = new List<Vector2>();
        this.tiles = new List<GameObject>();

        this.InitializeTiles();
    }

    void Update()
    {
        ObjectSelector2D.CheckForObjectDetection(hit => {
            string objectName = hit.collider.gameObject.name;
            if(objectName.Contains("Tile"))
            {
                Tile tileScript = hit.collider.gameObject.GetComponent<Tile>();
                tileScript.selected = true;
            }
        }, 50f);
    }
}
