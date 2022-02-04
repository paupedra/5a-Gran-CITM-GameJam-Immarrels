using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum OreType
{
    ROCK,
    METAL,
    COAL,
    CLAY
}

public class OreBehaviour : MonoBehaviour
{
    PlayerController player;

    public OreType type = OreType.ROCK;

    public int maxHp =3;
    public int hp = 3; //Amount of times it can be hit
    public int orePerHit = 1;

    bool exhausted;

    public MeshFilter childMesh;

    public Mesh fullHPModel;
    public Mesh halveHPModel;
    public Mesh lowHPModel;
    public Mesh exhaustedModel;

    CapsuleCollider collider;

    float respawnTimer = 0;
    public float respawnTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        collider = gameObject.GetComponent<CapsuleCollider>();
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
                childMesh.mesh = fullHPModel;
                exhausted = false;
            }

            respawnTimer += Time.deltaTime;
        }
    }

    public void ReceiveHit()
    {
        if(!exhausted)
        {
            switch (type)
            {
                case OreType.COAL:

                    player.coal+= orePerHit;

                    break;

                case OreType.METAL:
                    player.metal += orePerHit;
                    break;

                case OreType.ROCK:
                    player.rock += orePerHit;
                    break;

                case OreType.CLAY:
                    player.clay += orePerHit;
                    break;
            }

            hp--;

            if (hp == 2)
            {
                //change to second model
                childMesh.mesh = halveHPModel;
            }

            if (hp == 1)
            {
                //change to third model
                childMesh.mesh = lowHPModel;
            }

            if (hp == 0)
            {
                //start respawn timer
                childMesh.mesh = exhaustedModel;
                exhausted = true;
            }
        }
       

    }
}
