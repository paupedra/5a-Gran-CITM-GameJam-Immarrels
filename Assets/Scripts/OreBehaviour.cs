using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OreType
{
    ROCK,
    METAL,
    COAL
}

public class OreBehaviour : MonoBehaviour
{
    PlayerController player;

    OreType type = OreType.ROCK;

    public int hp = 3; //Amount of times it can be hit
    public int orePerHit = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveHit()
    {

    }
}
