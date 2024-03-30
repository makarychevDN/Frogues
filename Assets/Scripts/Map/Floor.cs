using FroguesFramework;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] public List<Cell> allCells;

    [SerializeField] private Unit player;

    public void Init()
    {
        if (needToGenerateMap)
        {
            globalTilemap.ClearAllTiles();
            floorGenerator.GenerateFloor();
        }

        map.Init(rooms);
        allCells = map.allCells;
        pathFinder.Init();
        SubscribeToEachCellToSwichRooms();


        startRoom.CenterCell.Content = player;
        player.CurrentCell = startRoom.CenterCell;
        player.transform.position = startRoom.transform.position;
        player.Init();

    }

    private void SubscribeToEachCellToSwichRooms()
    {
        foreach (Room room in rooms)
        {
            foreach (Cell cell in room.AllCells)
            {
                cell.OnSomeoneSteppedInMyRoom.AddListener(TryToSwitchRoom);
            }
        }
    }

    private void TryToSwitchRoom(Unit unit, Room targetRoom)
    {
        if (unit != player)
            return;

        foreach (Room room in rooms)
        {
            room.Enable(room == targetRoom);
        }

        foreach(Cell cell in allCells)
        {
            if (cell.ParentRoom == targetRoom)
                continue;

            if (cell.CellNeighbours.GetAllNeighbors().Any(neighbor => neighbor.ParentRoom == targetRoom))
                cell.gameObject.SetActive(true);
        }
    }

    public Room GetTheFirstRoomOnTheFloor()
    {
        return rooms[0];
    }

    public Tilemap GlobalTilemap => globalTilemap;
    public Map Map => map;
    public PathFinder PathFinder => pathFinder;
}
