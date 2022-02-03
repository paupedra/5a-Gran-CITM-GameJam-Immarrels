using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile
{
    public GameObject tileObject;

    public HexTileManager hexTileManager;
}

public class HexGridManager : MonoBehaviour
{
    public GameObject hexTile; //prefab for the hexagonal tiles

    public GameObject metalOre;
    public GameObject coalOre;
    public GameObject rockOre;
    public GameObject pavement;
    public GameObject sawmill;
    public GameObject refinery;
    public GameObject quarry;

    public int gridWidth = 10;
    public int gridHeight = 10;

    public float radius = 0.24f;

    public HexTile[] tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new HexTile[gridWidth * gridHeight];

        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        //generate grid and spawn in tiles

        float a = Mathf.Sqrt(radius * radius - (radius / 2) * (radius / 2));

        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth; x++)
            {

                GameObject newTile;

                if (y % 2 == 0)
                {
                    newTile = Instantiate(hexTile, new Vector3(x * a * 2, 0, y * (radius + radius / 2)), Quaternion.Euler(new Vector3(0, 30, 0)));
                }
                else
                {
                    newTile = Instantiate(hexTile, new Vector3(x * a * 2 + a, 0, y * (radius + radius / 2)), Quaternion.Euler(new Vector3(0, 30, 0)));
                }

                newTile.name = (x + y * gridWidth).ToString();
                

                HexTile newHexTile = new HexTile();

                newHexTile.tileObject = newTile;
                newHexTile.hexTileManager = newTile.GetComponent<HexTileManager>();
                newHexTile.hexTileManager.gridManager = this;
                newHexTile.hexTileManager.x = x;
                newHexTile.hexTileManager.y = y;

                if(x+y*gridWidth != 0)
                {
                    newHexTile.hexTileManager.SetTileOre();
                }
                

                newHexTile.hexTileManager.SetTileActive(false);

                tiles[x + y * gridWidth] = newHexTile;
            }
        }

        for (int i = 0; i < gridWidth * gridHeight; i++)
        {
            if(i==10)
            {
                Debug.Log("10");
            }
            tiles[i].hexTileManager.FindNeighbours();
        }

        tiles[0].hexTileManager.SetTileActive(true); //Starting tile
    }
}
