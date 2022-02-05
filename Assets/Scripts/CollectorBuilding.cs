using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorBuilding : MonoBehaviour
{
    public HexTileType collectorType;

    public float productionTime = 1; //Time it takes to produce an ore

    float productionTimer = 0;

    public int orePerProduction = 0;

    PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        productionTimer += Time.deltaTime;

        if(productionTimer >= productionTime)
        {
            productionTimer = 0;

            switch(collectorType)
            {
                case HexTileType.ROCK:
                    player.rock += 1;
                    break;

                case HexTileType.METAL:
                    player.metal += 1;
                    break;

                case HexTileType.COAL:
                    player.coal += 1;
                    break;
            }
        }

    }


}
