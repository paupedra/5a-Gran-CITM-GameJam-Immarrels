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
    public FollowTarget camera;
    GameObject player;

    public GameObject hexTile; //prefab for the hexagonal tiles

    public GameObject metalOre;
    public GameObject coalOre;
    public GameObject rockOre;
    public GameObject townHall;
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
        player = GameObject.Find("Player");

        tiles = new HexTile[gridWidth * gridHeight];

        GenerateGrid();

        SetStartingTiles();
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

                newHexTile.hexTileManager.SetTileOre();

                newHexTile.hexTileManager.SetTileActive(false);

                tiles[x + y * gridWidth] = newHexTile;
            }
        }

        for (int i = 0; i < gridWidth * gridHeight; i++)
        {
            tiles[i].hexTileManager.FindNeighbours();
        }

    }

    void SetStartingTiles()
    {
        //Spawn player in center
        Vector2 centerTile = new Vector2(gridWidth / 2, gridHeight / 2);

        //SetUp 4 center tiles to start (predetermined)
        tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].hexTileManager.SetTileActive(true); // x y (Empty)
        tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].hexTileManager.Build(HexTileType.EMPTY,Quaternion.identity);

        tiles[(int)centerTile.x + 1 + (int)centerTile.y * gridWidth].hexTileManager.SetTileActive(true); // x +1 y (Rock)
        tiles[(int)centerTile.x + 1 + (int)centerTile.y * gridWidth].hexTileManager.SetTileOre(HexTileType.ROCK, 3);

        tiles[(int)centerTile.x - 1 + (int)centerTile.y * gridWidth].hexTileManager.SetTileActive(true); // x -1 y (TownHall)
        tiles[(int)centerTile.x - 1 + (int)centerTile.y * gridWidth].hexTileManager.Build(HexTileType.TOWNHALL, Quaternion.identity);

        tiles[(int)centerTile.x +1 + ((int)centerTile.y + 1) * gridWidth].hexTileManager.SetTileActive(true); // x +1 y +1 (Empty)
        tiles[(int)centerTile.x + 1 + ((int)centerTile.y + 1) * gridWidth].hexTileManager.Build(HexTileType.EMPTY, Quaternion.identity);

        tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].hexTileManager.UnlockWallsArroundTile(tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].hexTileManager);
        tiles[(int)centerTile.x + 1 + ((int)centerTile.y + 1) * gridWidth].hexTileManager.UnlockWallsArroundTile(tiles[(int)centerTile.x + 1 + ((int)centerTile.y + 1) * gridWidth].hexTileManager);

        player.transform.SetPositionAndRotation(new Vector3(tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].tileObject.transform.position.x, 0.7f, tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].tileObject.transform.position.z), Quaternion.identity);
        camera.transform.SetPositionAndRotation(new Vector3(camera.transform.position.x + player.transform.position.x, camera.transform.position.y + player.transform.position.y, camera.transform.position.z + player.transform.position.z), camera.transform.rotation);
        camera.cameraOffset = camera.transform.position - camera.target.transform.position;
    }
}
