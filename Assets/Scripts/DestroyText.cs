using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyText : MonoBehaviour
{

    float seconds = 1.0f;
    void Start()
    {
        Destroy(gameObject, seconds);
    }

}
