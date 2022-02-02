using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - target.transform.position;
    }

   
    void Update()
    {
        Vector3 newPos = target.transform.position + cameraOffset;
        transform.position = newPos;
    }
}
