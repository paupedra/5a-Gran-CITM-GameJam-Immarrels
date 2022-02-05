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

    public GameObject floatingText;

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
                    player.rock += orePerProduction;
                    break;

                case HexTileType.METAL:
                    player.metal += orePerProduction;
                    break;

                case HexTileType.COAL:
                    player.coal += orePerProduction;
                    break;
            }

            ShowOreCollected(orePerProduction.ToString());
        }

    }

    void ShowOreCollected(string text)
    {
        GameObject obj = Instantiate(floatingText, transform.position, Quaternion.identity);
        string finalText = "+" + text;
        obj.GetComponentInChildren<TextMesh>().text = finalText;
    }

}
