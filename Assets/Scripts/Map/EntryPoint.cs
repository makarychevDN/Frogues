using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using TMPro;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    [SerializeField] private Room hub;
    [SerializeField] private List<Room> roomsPrefabs;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private Unit _metaPlayer;
    [SerializeField] private AbilitiesPanel _abilitiesPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private UnitDescriptionPanel unitDescriptionPanel;
    [SerializeField] private int score;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private TMP_Text scoreText;
    private int _roomsCount;

    public bool IsHub => _currentRoom == hub;
    public PathFinder PathFinder => _currentRoom.PathFinder;
    public Map Map => _currentRoom.Map;
    public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;
    public bool PauseIsActive => pausePanel.activeSelf;
    public UnitDescriptionPanel UnitDescriptionPanel => unitDescriptionPanel;
    public int Score => score;
    public bool NeedToShowUnitsUI => UnitsQueue.IsUnitCurrent(_metaPlayer) 
                                     && !_metaPlayer.MovementAbility.IsMoving 
                                     && !CurrentlyActiveObjects.SomethingIsActNow;

    public Vector3 CenterOfRoom => _currentRoom.CenterOfRoom;

    public void ActivateExit() => _currentRoom.ActivateExit();

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
        var newRoom = Instantiate(roomsPrefabs[_roomsCount]);
        _roomsCount++;
        _currentRoom.Deactivate();
        _currentRoom = newRoom;
        _currentRoom.Init(_metaPlayer);
        _metaPlayer.ActionPoints.SetPoints(8);
        _metaPlayer.Health.TakeHealing(33);
        waveSpawner.ResetRoundsTimer();
    }

    public void IncreaseScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }

    private void Update()
    {
        foreach (var ableToDisablePreVisualization in FindObjectsOfType<MonoBehaviour>().OfType<IAbleToDisablePreVisualization>())
        {
            ableToDisablePreVisualization.DisablePreVisualization();
        }

        if (PauseIsActive)
            return;

        _currentRoom.UnitsQueue.ActForCurrentUnit();
    }
}
