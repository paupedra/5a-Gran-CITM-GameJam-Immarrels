using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    Rigidbody body;
    public float speed = 5;

    HexGridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        gridManager = GameObject.Find("HexGridManager").GetComponent<HexGridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector3(transform.forward.x * speed,0,transform.forward.z * speed );
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "HaxTile")
        {
            
        }
    }
}
