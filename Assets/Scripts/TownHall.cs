using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
    PlayerController player;

    public int rockCompleteCost = 300;
    public int metalCompleteCost = 120;
    public int coalCompleteCost = 45;

    bool rockCompleted = false;
    bool metalCompleted = false;
    bool coalCompleted = false;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(coalCompleted == false)
            player.coalCompleteCostText.text = coalCompleteCost.ToString();
        if (metalCompleted == false)
            player.metalCompleteCostText.text = metalCompleteCost.ToString();
        if (rockCompleted == false)
            player.rockCompleteCostText.text = rockCompleteCost.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Open Menu
            player.townHallMenu.SetActive(true);
            player.townHallMenu.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetKey("q"))
            {
                //Open TownHall Menu
                if(rockCompleted == false)
                {
                    if (player.rock >= 0 && rockCompleteCost >= 0)
                    {
                        int x = player.rock;

                        if (player.rock >= rockCompleteCost)
                        {
                            player.rock -= rockCompleteCost;
                            rockCompleteCost -= rockCompleteCost;
                        }
                        else
                        {
                            rockCompleteCost -= x;
                            player.rock -= x;
                        }
 
                    }
                }
                if (coalCompleted == false && coalCompleteCost >= 0)
                {
                    if (player.coal >= 0 && coalCompleteCost >= 0)
                    {
                        int x = player.coal;

                        if (player.coal >= coalCompleteCost)
                        {
                            player.coal -= coalCompleteCost;
                            coalCompleteCost -= coalCompleteCost;
                        }
                        else
                        {
                            coalCompleteCost -= x;
                            player.coal -= x;
                        }

                    }
                }
                if (metalCompleted == false && metalCompleteCost >= 0)
                {
                    if (player.metal >= 0 && metalCompleteCost >= 0)
                    {
                        int x = player.metal;

                        if (player.metal >= metalCompleteCost)
                        {
                            player.metal -= metalCompleteCost;
                            metalCompleteCost -= metalCompleteCost;
                        }
                        else
                        {
                            metalCompleteCost -= x;
                            player.metal -= x;
                        }

                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Close Menu
            player.townHallMenu.SetActive(false);
        }
    }
}
