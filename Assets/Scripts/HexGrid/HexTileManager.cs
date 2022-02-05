using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexTileType
{
    ROCK,
    COAL,
    METAL,
    CLAY,
    PAVEMENT,
    TOWNHALL,
    REFINERY, //Clay into bricks
    QUARRY, //Rock
    MINE, //Coal
    FOUNDARY, //Metal
    EMPTY,
    NONE,
}

public class HexTileManager : MonoBehaviour
{
    public HexTileType tileType = HexTileType.EMPTY;

    GameObject[] containedObjects; //any contained objects that will be destroyed upon new construction

    public int x;
    public int y;

    public bool active = false;

    public bool corrupted = false;
    public float corruptionTime = 5;
    float corruptionTimer = 0;

    public float oreHeight= 0.5f;

    public Material uncorruptedMaterial;
    public Material corruptedMaterial;

    public HexGridManager gridManager;

    public BoxCollider unlockUpRight;
    public BoxCollider unlockRight;
    public BoxCollider unlockUpLeft;
    public BoxCollider unlockDownLeft;
    public BoxCollider unlockDownRight;
    public BoxCollider unlockLeft;

    public BoxCollider wallUpRight;
    public BoxCollider wallRight;
    public BoxCollider wallUpLeft;
    public BoxCollider wallDownLeft;
    public BoxCollider wallDownRight;
    public BoxCollider wallLeft;

    public HexTileManager tileUpRight;
    public HexTileManager tileRight;
    public HexTileManager tileUpLeft;
    public HexTileManager tileDownLeft;
    public HexTileManager tileDownRight;
    public HexTileManager tileLeft;

    private void Awake()
    {
        containedObjects = new GameObject[4];
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Update Corruption
        if(corrupted)
        {
            UpdateCorruption();
            
        }

    }

    public void CorruptTile()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().material = corruptedMaterial;
        corrupted = true;
        gridManager.corruptedTiles++;
    }

    public void UncorruptTile()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().material = uncorruptedMaterial;
        corrupted = false;
        gridManager.corruptedTiles--;
    }

    public void SetTileActive(bool _active)
    {
        if(_active)
        {
            active = true;
            gameObject.SetActive(true);
        }
        else
        {
            active = false;
            gameObject.SetActive(false);
        }
    }

    void UpdateCorruption()
    {
        corruptionTimer += Time.deltaTime;

        if (corruptionTimer >= corruptionTime)
        {
            int a = Random.Range(0, 2);
            if(a == 0)
            {
                return;
            }

            bool _corruped = false;
            int tilesChecked = 0;
            int i = 0;
            while (!_corruped && tilesChecked < 6)
            {
                int rng = i; //Random.Range(0, 7);
                corruptionTimer = 0;
                switch (rng)
                {
                    case 1:
                        if (tileUpRight != null)
                        {
                            if (!tileUpRight.corrupted)
                            {
                                tileUpRight.CorruptTile();
                                _corruped = true;
                            }
                        }

                        tilesChecked++;
                        break;
                    case 2:
                        if (tileRight != null)
                        {
                            if (!tileRight.corrupted)
                            {
                                tileRight.CorruptTile();
                                _corruped = true;
                            }
                        }
                        tilesChecked++;
                        break;
                    case 3:
                        if (tileUpLeft != null)
                        {
                            if (!tileUpLeft.corrupted)
                            {
                                tileUpLeft.CorruptTile();
                                _corruped = true;
                            }
                        }
                        tilesChecked++;
                        break;
                    case 4:
                        if (tileLeft != null)
                        {
                            if (!tileLeft.corrupted)
                            {
                                tileLeft.CorruptTile();
                                _corruped = true;
                            }
                        }
                        tilesChecked++;
                        break;
                    case 5:
                        if (tileDownRight != null)
                        {
                            if (!tileDownRight.corrupted)
                            {
                                tileDownRight.CorruptTile();
                                _corruped = true;
                            }
                        }
                        tilesChecked++;
                        break;
                    case 6:
                        if (tileDownLeft != null)
                        {
                            if (!tileDownLeft.corrupted)
                            {
                                tileDownLeft.CorruptTile();
                                _corruped = true;
                            }
                        }
                        tilesChecked++;
                        break;
                }
                i++;
            }

        }
    }

    public void Build(HexTileType type, Quaternion rotation)
    {
        if(tileType != HexTileType.TOWNHALL)
        {
            for (int i = 0; i < containedObjects.Length; i++)
            {
                Destroy(containedObjects[i]);
            }

            switch (type)
            {
                case HexTileType.TOWNHALL:
                    containedObjects[0] = Instantiate(gridManager.townHall, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), rotation, gameObject.transform);
                    tileType = HexTileType.TOWNHALL;
                    break;

                case HexTileType.PAVEMENT:
                    containedObjects[0] = Instantiate(gridManager.pavement, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), rotation, gameObject.transform);
                    tileType = HexTileType.PAVEMENT;
                    break;

                case HexTileType.QUARRY:
                    containedObjects[0] = Instantiate(gridManager.quarry, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), rotation, gameObject.transform);
                    tileType = HexTileType.QUARRY;
                    break;

                case HexTileType.FOUNDARY:
                    containedObjects[0] = Instantiate(gridManager.foundary, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), rotation, gameObject.transform);
                    tileType = HexTileType.FOUNDARY;
                    break;
                case HexTileType.MINE:
                    containedObjects[0] = Instantiate(gridManager.mine, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), rotation, gameObject.transform);
                    tileType = HexTileType.MINE;
                    break;

                case HexTileType.REFINERY:
                    containedObjects[0] = Instantiate(gridManager.refinery, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), rotation, gameObject.transform);
                    tileType = HexTileType.REFINERY;
                    break;
            }
        }
    }

    public void SetTileOre(HexTileType forcedType = HexTileType.NONE, int forcedAmount = 0)
    {
        if(forcedType == HexTileType.NONE)
        {
            int diffX = (int)Mathf.Abs( gridManager.centerTile.x - x); //Center tile 100 ,100
            int diffY = (int)Mathf.Abs( gridManager.centerTile.y - y); //current 101,100
            int a = diffX + diffY;
            
            int circle = 0;


            if (diffX + diffY > 2 && diffX + diffY <= 5)
            {
                circle = 1;
              
            }

            if (diffX + diffY > 5 && diffX + diffY <= 10)
            {
                circle = 2;
            }

            if (diffX + diffY > 10)
            {
                circle = 3;
            }
            

            int rng = Random.Range(0, 101);
            //Debug.Log(string.Concat("Rng was:" ,rng.ToString()));

            int[] chances = new int[] { 15, 55, 15, 10, 5 };
            int sum = 0;

            switch (circle)
            {
                case 0:
                    chances = new int[] { 0, 100, 0,0, 0 };
                    break;
                case 1:
                    chances = new int[] { 15, 50, 15, 10, 10 };

                    break;

                case 2:
                    chances = new int[] { 15, 35, 22, 13, 15 };
                    break;

                case 3:
                    chances = new int[] { 15, 20, 26, 15, 24 };
                    break;
            }

            


            for (int i = 0; i < chances.Length; i++)
            {
                sum += chances[i];

                if (rng <= sum)
                {
                    switch (i)
                    {
                        case 0:
                            forcedType = HexTileType.EMPTY;
                            Debug.Log("Empty");
                            break;
                        case 1:
                            forcedType = HexTileType.ROCK;
                            Debug.Log("Rock");
                            break;
                        case 2:
                            forcedType = HexTileType.COAL;
                            break;
                        case 3:
                            forcedType = HexTileType.METAL;
                            break;
                        case 4:
                            forcedType = HexTileType.CLAY;
                            break;
                    }
                    break;
                }

            }
        }
        

        for (int i = 0; i < containedObjects.Length; i++)
        {
            Destroy(containedObjects[i]);
        }

        if(forcedType == HexTileType.EMPTY)
        { 
            return; 
        }

        int oreNum = 0;
        
        if (forcedAmount != 0)
        {
            oreNum = forcedAmount;
        }
        else
        {
            oreNum = Random.Range(1, 4);
        }

        int ore;

        if (forcedType != HexTileType.NONE)
        {
            ore = (int)forcedType;
        }
        else
        {
            ore = Random.Range(0, 4);
        }

        GameObject orePrefab = gridManager.rockOre;

        switch (ore)
        {
            case 0: //Rock
                tileType = HexTileType.ROCK;
                orePrefab = gridManager.rockOre;

                break;

            case 1: //Coal
                tileType = HexTileType.COAL;
                orePrefab = gridManager.coalOre;

                break;

            case 2: //Metal
                tileType = HexTileType.METAL;
                orePrefab = gridManager.metalOre;

                break;

            case 3: //Clay
                tileType = HexTileType.CLAY;
                orePrefab = gridManager.clayOre;

                break;
        }

        switch (oreNum)
        {
            case 1:

                containedObjects[0] = Instantiate(orePrefab, new Vector3(gameObject.transform.position.x, oreHeight, gameObject.transform.position.z), Quaternion.identity, gameObject.transform);

                break;

            case 2:
                containedObjects[0] = Instantiate(orePrefab, new Vector3(gameObject.transform.position.x + 0.5F, oreHeight, gameObject.transform.position.z), Quaternion.identity, gameObject.transform);
                containedObjects[1] = Instantiate(orePrefab, new Vector3(gameObject.transform.position.x, oreHeight, gameObject.transform.position.z), Quaternion.identity, gameObject.transform);
                break;

            case 3:
                containedObjects[0] = Instantiate(orePrefab, new Vector3(gameObject.transform.position.x+0.5f, oreHeight, gameObject.transform.position.z), Quaternion.identity, gameObject.transform);
                containedObjects[1] = Instantiate(orePrefab, new Vector3(gameObject.transform.position.x, oreHeight, gameObject.transform.position.z+ 0.5f), Quaternion.identity, gameObject.transform);
                containedObjects[2] = Instantiate(orePrefab, new Vector3(gameObject.transform.position.x, oreHeight, gameObject.transform.position.z - 0.5f), Quaternion.identity, gameObject.transform);
                break;
        }

        
    }

    public void FindNeighbours()
    {

        if( y % 2 == 0) //if even (0,2,4,6
        {

            if(x + 1 < gridManager.gridWidth)
            {
                tileRight = gridManager.tiles[x + 1 + y * gridManager.gridWidth].hexTileManager;
            }

            if(x != 0)
            {
                tileLeft = gridManager.tiles[x - 1 + y * gridManager.gridWidth].hexTileManager;
            }

            if (y + 1 < gridManager.gridHeight )
            {
                tileUpRight = gridManager.tiles[x + (y + 1) * gridManager.gridWidth].hexTileManager;

                if (x != 0)
                {
                    tileUpLeft = gridManager.tiles[x - 1 + (y + 1) * gridManager.gridWidth].hexTileManager;
                }
            }

            if (y != 0)
            {
                tileDownRight = gridManager.tiles[x  + (y - 1) * gridManager.gridWidth].hexTileManager;

                if (x != 0)
                {
                    tileDownLeft = gridManager.tiles[x - 1 + (y - 1) * gridManager.gridWidth].hexTileManager;
                }
            }

        }
        else //If Uneven
        {

            if (x + 1 < gridManager.gridWidth)
            {
                tileRight = gridManager.tiles[x + 1 + y * gridManager.gridWidth].hexTileManager;
            }

            if (x != 0)
            {
                tileLeft = gridManager.tiles[x - 1 + y * gridManager.gridWidth].hexTileManager;
            }

            if (y + 1 < gridManager.gridHeight  && x + 1 < gridManager.gridWidth )
            {
                tileUpRight = gridManager.tiles[x + 1 + (y + 1) * gridManager.gridWidth].hexTileManager;
            }

            if (y + 1 < gridManager.gridHeight )
            {
                tileUpLeft = gridManager.tiles[x + (y + 1) * gridManager.gridWidth].hexTileManager;
            }

            if (y != 0 && x + 1 < gridManager.gridWidth )
            {
                tileDownRight = gridManager.tiles[x + 1 + (y - 1) * gridManager.gridWidth].hexTileManager;
            }

            if (y != 0)
            {
                tileDownLeft = gridManager.tiles[x + (y - 1) * gridManager.gridWidth].hexTileManager;
            }
        }
    }

    public void UnlockWallsArroundTile(HexTileManager tile)
    {
        //Check if any of the contiguous tiles is active and deactivate all the walls / triggers

        if(tile.tileUpRight != null)
        {
            if(tile.tileUpRight.active)
            {
                tile.wallUpRight.gameObject.SetActive(false);
                tile.unlockUpRight.gameObject.SetActive(false);

                tile.tileUpRight.unlockDownLeft.gameObject.SetActive(false);
                tile.tileUpRight.wallDownLeft.gameObject.SetActive(false);
            }
        }

        if (tile.tileUpLeft != null)
        {
            if (tile.tileUpLeft.active)
            {
                tile.wallUpLeft.gameObject.SetActive(false);
                tile.unlockUpLeft.gameObject.SetActive(false);

                tile.tileUpLeft.unlockDownRight.gameObject.SetActive(false);
                tile.tileUpLeft.wallDownRight.gameObject.SetActive(false);
            }
        }

        if (tile.tileRight != null)
        {
            if (tile.tileRight.active)
            {
                tile.wallRight.gameObject.SetActive(false);
                tile.unlockRight.gameObject.SetActive(false);

                tile.tileRight.unlockLeft.gameObject.SetActive(false);
                tile.tileRight.wallLeft.gameObject.SetActive(false);
            }
        }

        if (tile.tileLeft != null)
        {
            if (tile.tileLeft.active)
            {
                tile.wallLeft.gameObject.SetActive(false);
                tile.unlockLeft.gameObject.SetActive(false);

                tile.tileLeft.unlockRight.gameObject.SetActive(false);
                tile.tileLeft.wallRight.gameObject.SetActive(false);
            }
        }

        if (tile.tileDownRight != null)
        {
            if (tile.tileDownRight.active)
            {
                tile.wallDownRight.gameObject.SetActive(false);
                tile.unlockDownRight.gameObject.SetActive(false);

                tile.tileDownRight.unlockUpLeft.gameObject.SetActive(false);
                tile.tileDownRight.wallUpLeft.gameObject.SetActive(false);
            }
        }

        if (tile.tileDownLeft != null)
        {
            if (tile.tileDownLeft.active)
            {
                tile.wallDownLeft.gameObject.SetActive(false);
                tile.unlockDownLeft.gameObject.SetActive(false);

                tile.tileDownLeft.unlockUpRight.gameObject.SetActive(false);
                tile.tileDownLeft.wallUpRight.gameObject.SetActive(false);
            }
        }

    }

    public void UnlockUpRightTile()
    {
        if(tileUpRight != null)
        {
            tileUpRight.SetTileActive(true);

            UnlockWallsArroundTile(tileUpRight);
        }
    }

    public void UnlockUpLeftTile()
    {
        if (tileUpLeft != null)
        {
            tileUpLeft.SetTileActive(true);

            UnlockWallsArroundTile(tileUpLeft);
        }
    }

    public void UnlockRightTile()
    {
        if (tileRight != null)
        {
            tileRight.SetTileActive(true);

            UnlockWallsArroundTile(tileRight);
        }
    }

    public void UnlockLeftTile()
    {
        if (tileLeft != null)
        {
            tileLeft.SetTileActive(true);

            UnlockWallsArroundTile(tileLeft);
        }
    }

    public void UnlockDownRightTile()
    {
        if (tileDownRight != null)
        {
            tileDownRight.SetTileActive(true);

            UnlockWallsArroundTile(tileDownRight);
        }
    }

    public void UnlockDownLeftTile()
    {
        if (tileDownLeft != null)
        {
            tileDownLeft.SetTileActive(true);

            UnlockWallsArroundTile(tileDownLeft);
        }
    }

}
