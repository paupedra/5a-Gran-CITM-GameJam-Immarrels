using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 10.0f;
    public Vector3 cameraOffset;

    Vector3 currentVel;

    float moveCamTimer = 0;
    float moveCamTime = 2;

    float moveSpeed = 1;

    bool endGame=false;

    void Start()
    {
        cameraOffset = transform.position - target.transform.position;
    }

   
    void LateUpdate()
    {
        
            Vector3 newPos = target.transform.position + cameraOffset;
            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, newPos, ref currentVel, smoothSpeed * Time.deltaTime);
            transform.position = smoothPos;

            if (gameObject.name == "MinimapCamera")
            {
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            }
            else
            {
                //transform.LookAt(target);
            }
        
        if(endGame)
        {
            moveCamTimer += Time.deltaTime;

            if(moveCamTimer <= moveCamTime)
            {
                cameraOffset.y -= moveSpeed * Time.deltaTime;
                cameraOffset.z += moveSpeed * Time.deltaTime;
            }
        }
        
    }

    public void StartEndGame()
    {
        endGame = true;
    }
}
