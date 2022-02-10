using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HexTile
{
    public GameObject tileObject;

    public HexTileManager hexTileManager;
}

public class HexGridManager : MonoBehaviour
{
    public bool unlockedFirst = false;
    public bool finishedTutorial = false;

    public FollowTarget camera;

    public FollowTarget minimapCamera;

    GameObject player;

    public int corruptedTiles = 0;
    public Text corruptionText;
    public Text nextCorruptionText;

    public GameObject hexTile; //prefab for the hexagonal tiles

    public GameObject metalOre;
    public GameObject coalOre;
    public GameObject rockOre;
    public GameObject clayOre;

    public GameObject townHall;
    public GameObject pavement;
    public GameObject refinery;

    public GameObject quarry;
    public GameObject mine;
    public GameObject foundary;

    public Vector2 centerTile;

    public float corruptionTime = 5;
    float corruptionTimer = 0;

    public int gridWidth = 10;
    public int gridHeight = 10;

    public float radius = 0.24f;

    public HexTile[] tiles;

    public bool showAllTiles = false;

    public AudioSource corruption;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        tiles = new HexTile[gridWidth * gridHeight];

        centerTile = new Vector2(gridWidth / 2, gridHeight / 2);

        GenerateGrid();

        SetStartingTiles();


    }

    // Update is called once per frame
    void Update()
    {
        corruptionText.text = string.Concat(((float)corruptedTiles / (float)(tiles.Length - 1) * 100).ToString("F2")," % Corruption");

        if(corruptedTiles >= tiles.Length-1)
        {
            Debug.Log("Loser game");
        }

        if(finishedTutorial)
        {
            corruptionTimer += Time.deltaTime;

            

            if (corruptionTimer >= (100 * 0.3) + corruptedTiles/10 - 25)
            {
                corruption.Play();

                for (int i = 0; i < tiles.Length; i++)
                {
                    if (tiles[i].hexTileManager.corrupted)
                    {
                        tiles[i].hexTileManager.UpdateCorruption();
                    }

                }
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (tiles[i].hexTileManager.toCorrupt)
                    {
                        tiles[i].hexTileManager.ToCorruptTrigger();
                    }

                }
                corruptionTimer = 0;
            }
            nextCorruptionText.text = string.Concat("Corruption expands in: ", (((100 * 0.3) + corruptedTiles / 10 - 25) - corruptionTimer).ToString("f2"), " s");
        }
        
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

                if(!showAllTiles)
                {
                    newHexTile.hexTileManager.SetTileActive(false);
                }


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

        player.transform.SetPositionAndRotation(new Vector3(tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].tileObject.transform.position.x, 0.5f, tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].tileObject.transform.position.z), Quaternion.identity);
        camera.transform.SetPositionAndRotation(new Vector3(camera.transform.position.x + player.transform.position.x, camera.transform.position.y + player.transform.position.y, camera.transform.position.z + player.transform.position.z), camera.transform.rotation);
        camera.cameraOffset = camera.transform.position - camera.target.transform.position;
        player.transform.SetPositionAndRotation(new Vector3(tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].tileObject.transform.position.x, 0.5f, tiles[(int)centerTile.x + (int)centerTile.y * gridWidth].tileObject.transform.position.z), Quaternion.identity);
        
        minimapCamera.transform.SetPositionAndRotation(new Vector3(minimapCamera.transform.position.x + player.transform.position.x, minimapCamera.transform.position.y + player.transform.position.y, minimapCamera.transform.position.z + player.transform.position.z), minimapCamera.transform.rotation);
        minimapCamera.cameraOffset = minimapCamera.transform.position - minimapCamera.target.transform.position;


        //Set Starting Corrupted Tile

        tiles[0].hexTileManager.CorruptTile();
        tiles[tiles.Length-1].hexTileManager.CorruptTile();
        //tiles[gridWidth-1].hexTileManager.CorruptTile();
        //tiles[(gridHeight-1) * gridWidth].hexTileManager.CorruptTile();
    }
}
