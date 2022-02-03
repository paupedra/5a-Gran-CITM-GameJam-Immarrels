using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    public float playerSpeed = 6.0f;
    public float playerTurnTime = 0.1f;

    float turnVelocity;

    void Start()
    {
        
    }


    void Update()
    {
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
        }
    }
}