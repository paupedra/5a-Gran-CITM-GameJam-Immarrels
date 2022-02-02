using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HexTile
{


    public int x;
    public int y;

    //pther attributes: occupied, player occupied...
}

public class HexGridManager : MonoBehaviour
{
    public GameObject hexTile; //prefab for the hexagonal tiles


    public int gridWidth = 10;
    public int gridHeight = 10;

    public float radius = 0.24f;

    HexTile[] tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new HexTile[gridWidth * gridHeight];

        //generate grid and spawn in tiles

        float a = Mathf.Sqrt(radius * radius - (radius / 2) * (radius / 2));

        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth;x++)
            {

                GameObject newTile;

                if (y % 2 == 0)
                {
                    newTile = Instantiate(hexTile, new Vector3(y * (radius + radius / 2) , 0, x * a * 2 ), Quaternion.Euler(new Vector3(0,0,0)));
                }
                else
                {
                    newTile = Instantiate(hexTile, new Vector3(y * (radius + radius / 2) , 0, x * a * 2 + a ), Quaternion.Euler(new Vector3(0, 0, 0)));
                }

                newTile.name = (x + y *gridWidth).ToString();

                HexTile newHexTile = new HexTile();

                newHexTile.x = x;
                newHexTile.y = y;

                tiles[x + y] = new HexTile();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
