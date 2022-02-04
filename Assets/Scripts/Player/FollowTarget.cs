using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 10.0f;
    public Vector3 cameraOffset;

    Vector3 currentVel;

    void Start()
    {
        cameraOffset = transform.position - target.transform.position;
    }

   
    void LateUpdate()
    {
        Vector3 newPos = target.transform.position + cameraOffset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, newPos, ref currentVel, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;
   
        if(gameObject.name == "MinimapCamera")
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else
        {
            //transform.LookAt(target);
        }
    }
}
