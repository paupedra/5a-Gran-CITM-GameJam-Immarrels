using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineryBehaviour : MonoBehaviour
{

    PlayerController player;

    public bool corrupted = false;
    AudioSource audioSrc;

    float useTimer = 0;
    float useTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSrc = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        useTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !corrupted)
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
        if (other.tag == "Player" && !corrupted)
        {
            if (Input.GetKey("e"))
            {
                if (useTimer >= useTime)
                {
               
                    player.brick += player.clay / 2;
                    player.clay = player.clay % 2;
                    audioSrc.Play();
                    useTimer = 0;
                }
                
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
