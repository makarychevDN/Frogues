using FroguesFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Floor : MonoBehaviour
{
    [SerializeField] private Tilemap globalTilemap;
    [SerializeField] private Map map;
    [SerializeField] private FloorGenerator floorGenerator;
    [SerializeField] private PathFinder pathFinder;
    [SerializeField] private bool needToGenerateMap = true;
    [SerializeField] private List<Room> rooms;

    public void Init()
    {
        if (needToGenerateMap)
        {
            globalTilemap.ClearAllTiles();
            floorGenerator.GenerateFloor();
        }
    }

    public void Generate()
    {

    }

    public Room GetTheFirstRoomOnTheFloor()
    {
        return rooms[0];
    }
}
