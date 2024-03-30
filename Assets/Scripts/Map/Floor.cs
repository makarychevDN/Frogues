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
    [SerializeField] private Room startRoom;

    [SerializeField] private Unit player;

    public void Init()
    {
        if (needToGenerateMap)
        {
            globalTilemap.ClearAllTiles();
            floorGenerator.GenerateFloor();
        }

        map.Init(rooms);
        pathFinder.Init();

        startRoom.CenterCell.Content = player;
        player.CurrentCell = startRoom.CenterCell;
        player.transform.position = startRoom.transform.position;
        player.Init();

    }

    public Room GetTheFirstRoomOnTheFloor()
    {
        return rooms[0];
    }

    public Tilemap GlobalTilemap => globalTilemap;
    public Map Map => map;
    public PathFinder PathFinder => pathFinder;
}
