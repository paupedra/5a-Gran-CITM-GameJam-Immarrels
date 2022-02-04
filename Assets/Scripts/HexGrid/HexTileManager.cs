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
}

public class HexTileManager : MonoBehaviour
{
    public HexTileType tileType = HexTileType.EMPTY;

    GameObject[] containedObjects; //any contained objects that will be destroyed upon new construction

    public int x;
    public int y;

    public bool active = false;

    public float oreHeight= 0.5f;

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

    public void SetTileOre(HexTileType forcedType = HexTileType.EMPTY, int forcedAmount = 0)
    {
        int diffX = (int)Mathf.Abs(x - gridManager.centerTile.x);
        int diffY = (int)Mathf.Abs(y - gridManager.centerTile.y);

        int circle = 0;

        if(diffX + diffY <= 7)
        {

        }

        //Inner circle
        //15% empty tile
        //55% rock tile
        //15% coal
        //10% metal
        //5% Clay

        //Mid Circle
        //15% empty tile
        //40% rock tile
        //22% coal
        //13% metal
        //10% Clay

        //Outer Circle
        //15% empty tile
        //30% rock tile
        //26% coal
        //15% metal
        //14% Clay

        for (int i = 0; i < containedObjects.Length; i++)
        {
            Destroy(containedObjects[i]);
        }

        int oreNum = 0;
        
        if (forcedAmount != 0)
        {
            oreNum = forcedAmount;
        }
        else
        {
            oreNum = Random.Range(0, 5);
        }

        
        if (oreNum > 0)
        {
            int ore;

            if (forcedType != HexTileType.EMPTY)
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
