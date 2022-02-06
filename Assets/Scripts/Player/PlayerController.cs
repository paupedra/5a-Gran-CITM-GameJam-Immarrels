using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    bool playerMode = true; // true = Mine Mode , false = Build Mode (Build mode is active whilst build menu is open)

    GameManager gameManager;
    HexGridManager gridManager;
    public CharacterController controller;
    public Camera camera;

    public AudioSource footStepSound;
    public AudioSource buildSound;
    public AudioSource unlockSound;
    public AudioSource shootSound;

    float footStepTimer = 0;
    float footStepTime = 0.4f;

    public GameObject pickIcon;
    public GameObject buildIcon;

    public float playerSpeed = 6.0f;
    public float playerTurnTime = 0.1f;

    public int rock = 0;
    public int coal = 0;
    public int metal = 0;
    public int clay = 0;
    public int brick = 0;

    public int shootCost = 10;

    //Building Costs
    int[] quarryCosts = new int[] {35,9,0};
    int[] mineCosts = new int[] { 25, 15, 6 };
    int[] foundaryCosts = new int[] { 25, 11, 14 };
    int[] refineryCosts = new int[] { 50, 0, 0 };

    public Text coalText;
    public Text metalText;
    public Text rockText;
    public Text clayText;
    public Text brickText;

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
    public Material redTransparentMat;
    GameObject previewBuilding;

    public GameObject miningAreaObject;

    public GameObject projectile;

    public GameObject corruptTutorialMenu;

    public GameObject buildMenu;
    public GameObject refineryCostMenu;
    public GameObject townHallMenu;
    public Text clayCostText;
    public Text bricksProducedText;

    public Text coalCostText;
    public Text metalCostText;
    public Text rockCostText;

    public Text coalCompleteCostText;
    public Text metalCompleteCostText;
    public Text rockCompleteCostText;

    HexTileType buildingType = HexTileType.QUARRY;

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

        pickIcon.SetActive(true);
        buildIcon.SetActive(false);

        refineryCostMenu.SetActive(false);
        townHallMenu.SetActive(false);

        buildMenu.SetActive(false);
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

            coalText.text = coal.ToString();
            metalText.text = metal.ToString();
            rockText.text = rock.ToString();
            clayText.text = clay.ToString();
            brickText.text = brick.ToString();

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

            if(coalCostUnlock < 0)
            {
                coalCostUnlock = 0;
            }
            if (metalCostUnlock < 0)
            {
                metalCostUnlock = 0;
            }
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

            if(playerMode)
            {
                pickIcon.SetActive(true);
                buildIcon.SetActive(false);
                buildMenu.SetActive(false);
            }
            else
            {
                pickIcon.SetActive(false);
                buildIcon.SetActive(true);
                buildMenu.SetActive(true);
            }

            if(!playerMode)
            {
                switch (buildingType)
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

                    case HexTileType.TOWNHALL:
                        previewBuilding = Instantiate(gridManager.refinery);
                        break;
                }

                SetTransparentMaterial();
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

            if (Input.GetKeyDown(KeyCode.Mouse0) && tileHit > 0 && tileHit < gridManager.gridWidth * gridManager.gridHeight && gridManager.tiles[tileHit].hexTileManager.active && !EventSystem.current.IsPointerOverGameObject())
            {
                if(tileHit == currentPlayerTile)
                {
                    return;
                }

                if(ComputeCostBuilding(buildingType,true))
                {
                    gridManager.tiles[tileHit].hexTileManager.Build(buildingType, Quaternion.identity);
                    buildSound.Play();
                }
            }
        }
    }

    bool ComputeCostBuilding(HexTileType type, bool spend) //Returns true if player can afford building
    {
        bool ret = false;

        switch(type)
        {
            case HexTileType.PAVEMENT:
                
                break;

            case HexTileType.FOUNDARY:
                if(foundaryCosts[0] <= rock && foundaryCosts[1] <= coal && foundaryCosts[2] <= metal)
                {
                    ret = true;

                    if(spend)
                    {
                        rock -= foundaryCosts[0];
                        coal -= foundaryCosts[1];
                        metal -= foundaryCosts[2];
                    }
                }
                break;

            case HexTileType.QUARRY:
                if (quarryCosts[0] <= rock && quarryCosts[1] <= coal && quarryCosts[2] <= metal)
                {
                    ret = true;
                    if (spend)
                    {
                        rock -= quarryCosts[0];
                        coal -= quarryCosts[1];
                        metal -= quarryCosts[2];
                    }
                }
                break;

            case HexTileType.MINE:
                if (mineCosts[0] <= rock && mineCosts[1] <= coal && mineCosts[2] <= metal)
                {
                    ret = true;
                    if (spend)
                    {
                        rock -= mineCosts[0];
                        coal -= mineCosts[1];
                        metal -= mineCosts[2];
                    }
                }
                break;

            case HexTileType.REFINERY:
                if (refineryCosts[0] <= rock && refineryCosts[1] <= coal && refineryCosts[2] <= metal)
                {
                    ret = true;
                    if (spend)
                    {
                        rock -= refineryCosts[0];
                        coal -= refineryCosts[1];
                        metal -= refineryCosts[2];
                    }
                }
                break;
        }

        return ret;
    }

    void SetTransparentMaterial()
    {

        MeshRenderer[] renderers = previewBuilding.GetComponentsInChildren<MeshRenderer>();

        if (ComputeCostBuilding(buildingType, false))
        {
            for(int i =0;i<renderers.Length;i++)
            {
                renderers[i].material = greenTransparentMat;
            }
            
        }
        else
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = redTransparentMat;
            }
        }


        if (previewBuilding.GetComponent<BoxCollider>() != null)
        {
            previewBuilding.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            previewBuilding.GetComponentInChildren<BoxCollider>().enabled = false;
        }

        if(previewBuilding.GetComponent<CollectorBuilding>() != null)
        {
            previewBuilding.GetComponent<CollectorBuilding>().enabled = false;
        }

        if (previewBuilding.GetComponent<RefineryBehaviour>() != null)
        {
            previewBuilding.GetComponent<RefineryBehaviour>().enabled = false;
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
            case HexTileType.TOWNHALL:
                if (buildingType != HexTileType.TOWNHALL)
                {
                    buildingType = HexTileType.TOWNHALL;
                    Destroy(previewBuilding);
                    previewBuilding = Instantiate(gridManager.townHall);
                    previewBuilding.transform.SetPositionAndRotation(new Vector3(gridManager.tiles[tileHit].tileObject.transform.position.x, 0.5f, gridManager.tiles[tileHit].tileObject.transform.position.z), Quaternion.identity);
                }
                break;
        }

        SetTransparentMaterial();
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

            //Shoot logic
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Shoot();
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

    void Shoot()
    {
        if(brick >= shootCost)
        {
            brick -= shootCost;
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y +30, transform.rotation.eulerAngles.z)));
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 30, transform.rotation.eulerAngles.z)));

            shootSound.Play();
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

            footStepTimer += Time.deltaTime;
            if(footStepTimer >= footStepTime)
            {
                footStepSound.Play();
                footStepTimer = 0;
            }
            
        }
        
        animator.SetFloat("Speed", magnitudeSpeed, 0.05f, Time.deltaTime);
        //-----
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "UnlockTrigger")
        {
            if (Input.GetKey("e"))
            {
                if (coal >= coalCostUnlock && rock >= rockCostUnlock && metal >= metalCostUnlock)
                {
                    coal -= coalCostUnlock;
                    rock -= rockCostUnlock;
                    metal -= metalCostUnlock;
                }
                else
                {
                    return;
                }


                if (other.gameObject.name == "UnlockTileUpRight")
                {
                    other.gameObject.GetComponentInParent<HexTileManager>().UnlockUpRightTile();
                    if(!gridManager.unlockedFirst)
                    {
                        gridManager.unlockedFirst = true;
                        other.gameObject.GetComponentInParent<HexTileManager>().tileUpRight.CorruptTile();
                        corruptTutorialMenu.SetActive(true);
                    }
                }

                if (other.gameObject.name == "UnlockTileUpLeft")
                {
                    other.gameObject.GetComponentInParent<HexTileManager>().UnlockUpLeftTile();
                    if (!gridManager.unlockedFirst)
                    {
                        gridManager.unlockedFirst = true;
                        other.gameObject.GetComponentInParent<HexTileManager>().tileUpLeft.CorruptTile();
                        corruptTutorialMenu.SetActive(true);
                    }
                }

                if (other.gameObject.name == "UnlockTileRight")
                {
                    other.gameObject.GetComponentInParent<HexTileManager>().UnlockRightTile();
                    if (!gridManager.unlockedFirst)
                    {
                        gridManager.unlockedFirst = true;
                        other.gameObject.GetComponentInParent<HexTileManager>().tileRight.CorruptTile();
                        corruptTutorialMenu.SetActive(true);
                    }
                }

                if (other.gameObject.name == "UnlockTileLeft")
                {
                    other.gameObject.GetComponentInParent<HexTileManager>().UnlockLeftTile();
                    if (!gridManager.unlockedFirst)
                    {
                        gridManager.unlockedFirst = true;
                        other.gameObject.GetComponentInParent<HexTileManager>().tileLeft.CorruptTile();
                        corruptTutorialMenu.SetActive(true);
                    }
                }

                if (other.gameObject.name == "UnlockTileDownRight")
                {
                    other.gameObject.GetComponentInParent<HexTileManager>().UnlockDownRightTile();
                    if (!gridManager.unlockedFirst)
                    {
                        gridManager.unlockedFirst = true;
                        other.gameObject.GetComponentInParent<HexTileManager>().tileDownRight.CorruptTile();
                        corruptTutorialMenu.SetActive(true);
                    }
                }

                if (other.gameObject.name == "UnlockTileDownLeft")
                {
                    other.gameObject.GetComponentInParent<HexTileManager>().UnlockDownLeftTile();
                    if (!gridManager.unlockedFirst)
                    {
                        gridManager.unlockedFirst = true;
                        other.gameObject.GetComponentInParent<HexTileManager>().tileDownLeft.CorruptTile();
                        corruptTutorialMenu.SetActive(true);
                    }
                }

                if (other.gameObject.tag == "UnlockTrigger" && unlockTileImage.activeSelf)
                {
                    unlockTileImage.SetActive(false);
                    animator.SetBool("Unlock", false);
                    unlockSound.Play();

                }

                unlockTimer = 0;
                unlock = true;
                animator.SetBool("Unlock", true);
            }
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
