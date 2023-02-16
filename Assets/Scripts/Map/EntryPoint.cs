using System;
using System.Collections.Generic;
using FroguesFramework;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    [SerializeField] private Room hub;
    [SerializeField] private List<Room> roomsPrefabs;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private Unit _metaPlayer;

    public PathFinder PathFinder => _currentRoom.PathFinder;
    public Map Map => _currentRoom.Map;
    public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;
    
    private void Awake()
    {
        Instance = this;
        _currentRoom = hub;
        _currentRoom.Init(_metaPlayer);
    }

    public void StartNextRoom()
    {
        var newRoom = Instantiate(roomsPrefabs.GetRandomElement());
        _currentRoom.gameObject.SetActive(false);
        _currentRoom = newRoom;
        _currentRoom.Init(_metaPlayer);
    }
}
