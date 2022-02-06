using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    Rigidbody body;
    public float speed = 5;
    public float lifeTime = 3;

    float lifeTimer = 0;

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

        lifeTimer += Time.deltaTime;

        if(lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "HaxTile")
        {
            gridManager.tiles[int.Parse(other.gameObject.name)].hexTileManager.UncorruptTile();
            if(!gridManager.finishedTutorial)
            {
                gridManager.finishedTutorial = true;
            }
        }
    }
}
