using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    bool playerMode = true; // true = Mine Mode , false = Build Mode (Build mode is active whilst build menu is open)

    GameManager gameManager;
    HexGridManager gridManager;
    public CharacterController controller;
    public Camera camera;


    public float playerSpeed = 6.0f;
    public float playerTurnTime = 0.1f;

    public int rock = 0;
    public int coal = 0;
    public int metal = 0;
    public int clay = 0;
    public int brick = 0;

    float turnVelocity;

    bool mining = false;
    float miningTimer=0.0f;
    public float miningTime = 0.75f;

    int tileHit = 0;
    int oldTileHit = -1;

    public int currentPlayerTile = 0;

    public Vector2 wantToUnlockTile;

    public int coalCostUnlock =0;
    public int metalCostUnlock=0;
    public int rockCostUnlock=0;

    public Material greenTransparentMat;
    GameObject previewBuilding;

    public GameObject miningAreaObject;

    public Text coalCostText;
    public Text metalCostText;
    public Text rockCostText;

    HexTileType buildingType = HexTileType.PAVEMENT;

    public GameObject unlockTileImage;
    public Vector3 imageOffset = new Vector3(0f, 0f, 5f);

    Animator animator;

    bool unlock = false;
    float unlockTimer = 0.0f;
    float unlockTime = 0.4f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gridManager = GameObject.Find("HexGridManager").GetComponent<HexGridManager>();
        miningAreaObject.SetActive(false);
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if(!gameManager.paused)
        {
            MovementUpdate();

            MiningUpdate();

            BuildingUpdate();

            CurrentTileRaycast();

            if (unlock == true)
            {
                
                unlockTimer += Time.deltaTime;

                if (unlockTimer >= unlockTime)
                {
                    unlock = false;
                    animator.SetBool("Unlock", false);
                }
            }

        }
    }

    void ComputeUnlockTileCost(Collider other)
    {
        wantToUnlockTile = new Vector2(0,0);
        bool even = false;

        if(gridManager.tiles[currentPlayerTile].hexTileManager.y % 2 == 0)
        {
            even = true;
        }

        if (other.gameObject.name == "UnlockTileUpRight")
        {
            if(even)
            {
                wantToUnlockTile.x = 0;
                wantToUnlockTile.y = 1;
            }
            else
            {
                wantToUnlockTile.x = 1;
                wantToUnlockTile.y = 1;
            }
        }

        if (other.gameObject.name == "UnlockTileUpLeft")
        {
            if (even)
            {
                wantToUnlockTile.x = -1;
                wantToUnlockTile.y = 1;
            }
            else
            {
                wantToUnlockTile.x = 1;
                wantToUnlockTile.y = 1;
            }
        }

        if (other.gameObject.name == "UnlockTileRight")
        {
            wantToUnlockTile.x = 1;
            wantToUnlockTile.y = 0;
        }

        if (other.gameObject.name == "UnlockTileLeft")
        {
            wantToUnlockTile.x = -1;
            wantToUnlockTile.y = 0;
        }

        if (other.gameObject.name == "UnlockTileDownRight")
        {
            if (even)
            {
                wantToUnlockTile.x = 0;
                wantToUnlockTile.y = -1;
            }
            else
            {
                wantToUnlockTile.x = 1;
                wantToUnlockTile.y = -1;
            }
        }

        if (other.gameObject.name == "UnlockTileDownLeft")
        {
            if (even)
            {
                wantToUnlockTile.x = -1;
                wantToUnlockTile.y = -1;
            }
            else
            {
                wantToUnlockTile.x = 0;
                wantToUnlockTile.y = -1;
            }
        }

        wantToUnlockTile.x += gridManager.tiles[currentPlayerTile].hexTileManager.x;
        wantToUnlockTile.y += gridManager.tiles[currentPlayerTile].hexTileManager.y;

        if(wantToUnlockTile.x + wantToUnlockTile.y * gridManager.gridWidth <= gridManager.tiles.Length)
        {
            //Compute the cost based on the difference in x and y
            int diffX = (int)Mathf.Abs(wantToUnlockTile.x - gridManager.centerTile.x);
            int diffY = (int)Mathf.Abs(wantToUnlockTile.y - gridManager.centerTile.y);

            rockCostUnlock = (int)(100*0.04*(diffX + diffY));
            coalCostUnlock = (int)(75 * 0.04 * ((diffX + diffY) - 7));
            metalCostUnlock = (int)(45 * 0.04 * ((diffX + diffY) - 14));
        }

    }

    void CurrentTileRaycast()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(0, -1, 0), Color.yellow);


        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y+1, transform.position.z), transform.TransformDirection(0, -1, 0), out hit, Mathf.Infinity, 1 << 6))
        {
            if (hit.transform.gameObject.tag == "HaxTile")
            {
                currentPlayerTile = int.Parse(hit.transform.gameObject.name);
            }
        }
        else
        {
            Debug.Log("No hit bro");
        }


    }

    void BuildingUpdate()
    {
        if(Input.GetKeyDown("b"))
        {
            playerMode = !playerMode;

            if(!playerMode)
            {
                switch(buildingType)
                {
                    case HexTileType.PAVEMENT:
                        previewBuilding = Instantiate(gridManager.pavement);
                        break;

                    case HexTileType.QUARRY:
                        previewBuilding = Instantiate(gridManager.quarry);
                        break;

                    case HexTileType.MINE:
                        previewBuilding = Instantiate(gridManager.mine);
                        break;

                    case HexTileType.FOUNDARY:
                        previewBuilding = Instantiate(gridManager.foundary);
                        break;

                    case HexTileType.REFINERY:
                        previewBuilding = Instantiate(gridManager.refinery);
                        break;
                }
            }
            else
            {
                Destroy(previewBuilding);
            }
        }

        if(!playerMode)
        {

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit,1000,1<<6))
            {
                if(hit.transform.gameObject.tag == "HaxTile")
                {
                    oldTileHit = tileHit;
                    tileHit = int.Parse(hit.transform.gameObject.name);
                }

            }

            if(oldTileHit != tileHit && tileHit > 0 && tileHit < gridManager.gridWidth * gridManager.gridHeight)
            {
                //move building preview
                previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x,0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z),Quaternion.identity);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && tileHit > 0 && tileHit < gridManager.gridWidth * gridManager.gridHeight && gridManager.tiles[tileHit].hexTileManager.active)
            {
                gridManager.tiles[tileHit].hexTileManager.Build(buildingType, Quaternion.identity);
            }
        }
    }

    public void SetBuilding(HexTileType type)
    {
        //Raycast mouse to tile (display building with transparency)
        switch(type)
        {
            case HexTileType.PAVEMENT:

                if (buildingType != HexTileType.PAVEMENT)
                {
                    buildingType = HexTileType.PAVEMENT;
                    Destroy(previewBuilding);
                    previewBuilding = Instantiate(gridManager.pavement);
                    previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x, 0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z), Quaternion.identity);
                }

                break;

            case HexTileType.REFINERY:
                if (buildingType != HexTileType.REFINERY)
                {
                    buildingType = HexTileType.REFINERY;
                    Destroy(previewBuilding);
                    previewBuilding = Instantiate(gridManager.refinery);
                    previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x, 0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z), Quaternion.identity);
                }
                break;

            case HexTileType.QUARRY:
                if (buildingType != HexTileType.QUARRY)
                {
                    buildingType = HexTileType.QUARRY;
                    Destroy(previewBuilding);
                    previewBuilding = Instantiate(gridManager.quarry);
                    previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x, 0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z), Quaternion.identity);
                }
                break;

            case HexTileType.MINE:
                if (buildingType != HexTileType.MINE)
                {
                    buildingType = HexTileType.MINE;
                    Destroy(previewBuilding);
                    previewBuilding = Instantiate(gridManager.mine);
                    previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x, 0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z), Quaternion.identity);
                }
                break;

            case HexTileType.FOUNDARY:
                if (buildingType != HexTileType.FOUNDARY)
                {
                    buildingType = HexTileType.FOUNDARY;
                    Destroy(previewBuilding);
                    previewBuilding = Instantiate(gridManager.foundary);
                    previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x, 0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z), Quaternion.identity);
                }
                break;
        }
    }

    public void StartBuildingMode()
    {
        playerMode = false;
    }

    public void StopBuildingMode()
    {
        playerMode = true;
    }

    void MiningUpdate()
    {
        if(playerMode)
        {
            //Mining Logic
            if (!mining && Input.GetKey("space"))
            {
                mining = true;
                miningAreaObject.SetActive(true);
                miningTimer = 0;
                animator.SetBool("Mining", true);
            }

        }

        if (mining)
        {
            miningTimer += Time.deltaTime;

            if (miningTimer >= miningTime)
            {
                mining = false;
                miningAreaObject.SetActive(false);
                animator.SetBool("Mining", false);
            }
        }
    }

    void MovementUpdate()
    {
        //Movement Logic
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float magnitudeSpeed = Mathf.Clamp01(direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, playerTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            
        }
        
        animator.SetFloat("Speed", magnitudeSpeed, 0.05f, Time.deltaTime);
        //-----
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKey("e"))
        {
            if(other.gameObject.name == "UnlockTileUpRight")
            {
                other.gameObject.GetComponentInParent<HexTileManager>().UnlockUpRightTile();
            }

            if (other.gameObject.name == "UnlockTileUpLeft")
            {
                other.gameObject.GetComponentInParent<HexTileManager>().UnlockUpLeftTile();
            }

            if (other.gameObject.name == "UnlockTileRight")
            {
                other.gameObject.GetComponentInParent<HexTileManager>().UnlockRightTile();
            }

            if (other.gameObject.name == "UnlockTileLeft")
            {
                other.gameObject.GetComponentInParent<HexTileManager>().UnlockLeftTile();
            }

            if (other.gameObject.name == "UnlockTileDownRight")
            {
                other.gameObject.GetComponentInParent<HexTileManager>().UnlockDownRightTile();
            }

            if (other.gameObject.name == "UnlockTileDownLeft")
            {
                other.gameObject.GetComponentInParent<HexTileManager>().UnlockDownLeftTile();
            }

            if (other.gameObject.tag == "UnlockTrigger" && unlockTileImage.activeSelf)
            {
                unlockTileImage.SetActive(false);
                animator.SetBool("Unlock", false);
            }

            unlockTimer = 0;
            unlock = true;
            animator.SetBool("Unlock", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "UnlockTrigger")
        {
            unlockTileImage.SetActive(true);
            unlockTileImage.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + imageOffset);

            //Compute cost
            ComputeUnlockTileCost(other);
            rockCostText.text = rockCostUnlock.ToString();

            if(metalCostUnlock > 0)
            {
                metalCostText.text = metalCostUnlock.ToString();
            }
            else
            {
                metalCostText.text = 0.ToString();
            }

            if (coalCostUnlock > 0)
            {
                coalCostText.text = coalCostUnlock.ToString();
            }
            else
            {
                coalCostText.text = 0.ToString();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "UnlockTrigger")
        {
            unlockTileImage.SetActive(false);
        }
    }
}
