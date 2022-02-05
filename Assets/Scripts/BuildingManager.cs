using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject player;
    PlayerController script;
    HexTileType type;

    private void Start()
    {
        script = player.GetComponent<PlayerController>();

    }

    public void BuildQuarry()
    {
        type = HexTileType.QUARRY;
        script.SetBuilding(type);
    }
    public void BuildMine()
    {
        type = HexTileType.MINE;
        script.SetBuilding(type);
    }
    public void BuildFoundry()
    {
        type = HexTileType.FOUNDARY;
        script.SetBuilding(type);
    }
    public void BuildRefinery()
    {
        type = HexTileType.REFINERY;
        script.SetBuilding(type);
    }
    public void BuildPavement()
    {
        type = HexTileType.PAVEMENT;
        script.SetBuilding(type);
    }
}
