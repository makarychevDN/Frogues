using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    [SerializeField] private Room hub;
    [SerializeField] private List<Room> roomsPrefabs;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private Unit _metaPlayer;
    [SerializeField] private AbilitiesPanel _abilitiesPanel;

    public PathFinder PathFinder => _currentRoom.PathFinder;
    public Map Map => _currentRoom.Map;
    public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;

    public bool NeedToShowUnitsUI => UnitsQueue.IsUnitCurrent(_metaPlayer) 
                                     && !_metaPlayer.MovementAbility.IsMoving 
                                     && !CurrentlyActiveObjects.SomethingIsActNow;

    public Unit MetaPlayer => _metaPlayer;

    private void Awake()
    {
        _abilitiesPanel.Init();
        Instance = this;
        _currentRoom = hub;
        _currentRoom.Init(_metaPlayer);
    }

    public void StartNextRoom()
    {
        var newRoom = Instantiate(roomsPrefabs.GetRandomElement());
        _currentRoom.Deactivate();
        _currentRoom = newRoom;
        _currentRoom.Init(_metaPlayer);
    }

    private void Update()
    {
        foreach (var ableToDisablePreVisualization in FindObjectsOfType<MonoBehaviour>().OfType<IAbleToDisablePreVisualization>())
        {
            ableToDisablePreVisualization.DisablePreVisualization();
        }

        _currentRoom.UnitsQueue.ActForCurrentUnit();
    }
}
