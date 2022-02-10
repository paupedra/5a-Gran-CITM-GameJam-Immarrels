using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TownHall : MonoBehaviour
{
    PlayerController player;

    GameManager gameManager;

    public int rockCompleteCost = 300;
    public int metalCompleteCost = 45;
    public int coalCompleteCost = 120;

    bool rockCompleted = false;
    bool metalCompleted = false;
    bool coalCompleted = false;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

        if(rockCompleted && coalCompleted && metalCompleted)
        {
            //DO stuff to end game in victory
            gameManager.won = true;
            player.townHallMenu.SetActive(false);
        }
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
                    if (player.rock >= 0 && rockCompleteCost > 0)
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
                    else if (rockCompleteCost == 0)
                    {
                        rockCompleted = true;
                    }
                }
                if (coalCompleted == false)
                {
                    if (player.coal >= 0 && coalCompleteCost > 0)
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
                    else if( coalCompleteCost == 0)
                    {
                        coalCompleted = true;
                    }
                }
                if (metalCompleted == false)
                {
                    if (player.metal >= 0 && metalCompleteCost > 0)
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
                    else if (metalCompleteCost == 0)
                    {
                        metalCompleted = true;
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
