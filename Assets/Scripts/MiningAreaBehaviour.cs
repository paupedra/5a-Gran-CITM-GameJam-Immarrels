using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAreaBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.gameObject.tag == "Ore")
        {
            other.gameObject.GetComponent<OreBehaviour>().ReceiveHit();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ore")
        {
            collision.gameObject.GetComponent<OreBehaviour>().ReceiveHit();
        }
    }
}
