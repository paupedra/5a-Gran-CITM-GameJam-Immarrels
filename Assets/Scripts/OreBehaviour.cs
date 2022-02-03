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

    public int maxHp =3;
    public int hp = 3; //Amount of times it can be hit
    public int orePerHit = 1;

    public MeshFilter childMesh;

    public Mesh fullHPModel;
    public Mesh halveHPModel;
    public Mesh lowHPModel;
    public Mesh exhaustedModel;

    float respawnTimer = 0;
    float respawnTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(hp <= 0)
        {
            if(respawnTimer>=respawnTime)
            {
                hp = maxHp;
                respawnTimer = 0;
                //change to full hp model
                childMesh.mesh = fullHPModel;
            }

            respawnTimer += Time.deltaTime;
        }
    }

    public void ReceiveHit()
    {
        hp--;

        if(hp == 2)
        {
            //change to second model
            childMesh.mesh = halveHPModel;
        }

        if(hp == 1)
        {
            //change to third model
            childMesh.mesh = lowHPModel;
        }

        if(hp == 0)
        {
            //start respawn timer
            childMesh.mesh = null;
        }

    }
}
