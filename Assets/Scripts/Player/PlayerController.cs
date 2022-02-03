using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    public CharacterController controller;

    public float playerSpeed = 6.0f;
    public float playerTurnTime = 0.1f;

    public int rock = 0;
    public int coal = 0;
    public int metal = 0;

    float turnVelocity;

    bool mining = false;
    float miningTimer=0.0f;
    public float miningTime = 0.75f;

    public GameObject miningAreaObject;
    BoxCollider miningAreaCollider;

    public GameObject unlockTileImage;
    public Vector3 imageOffset = new Vector3(0f, 0f, 5f);

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        miningAreaObject.SetActive(false);
        miningAreaCollider = miningAreaObject.GetComponent<BoxCollider>();

    }


    void Update()
    {
        if(!gameManager.paused)
        {
            //Movement Logic
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, playerTurnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            }
            //-----

            //Mining Logic
            if(!mining && Input.GetKey("space"))
            {
                mining = true;
                miningAreaObject.SetActive(true);
                miningTimer = 0;
            }

            if(mining)
            {
                miningTimer += Time.deltaTime;

                if(miningTimer>= miningTime)
                {
                    mining = false;
                    miningAreaObject.SetActive(false);
                }
            }
            //-----

        }
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
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "UnlockTrigger")
        {
            unlockTileImage.SetActive(true);
            unlockTileImage.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + imageOffset);
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
