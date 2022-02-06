using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineryBehaviour : MonoBehaviour
{

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Open Menu
            player.refineryCostMenu.SetActive(true);
            //player.refineryCostMenu.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), player.refineryCostMenu.transform.rotation);
            player.clayCostText.text = player.clay.ToString();
            player.bricksProducedText.text = (player.clay / 2).ToString();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if(Input.GetKey("e"))
            {
                player.brick += player.clay/2;
                player.clay = player.clay % 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Close Menu
            player.refineryCostMenu.SetActive(false);
        }
    }
}
