using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    [SerializeField] private Room hub;
    [SerializeField] private List<Room> roomsPrefabs;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private Unit _metaPlayer;
    [SerializeField] private AbilitiesPanel _abilitiesPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private UnitDescriptionPanel unitDescriptionPanel;
    [SerializeField] private int score;
    [SerializeField] private int deltaOfScoreToOpenExit = 200;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private TMP_Text scoreText;
    private int _roomsCount;
    private int _hashedScoreThenExitActivated;

    public bool CurrentRoomIsHub => _currentRoom == hub;
    public PathFinder PathFinder => _currentRoom.PathFinder;
    public Map Map => _currentRoom.Map;
    public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;
    public bool PauseIsActive => pausePanel.activeSelf;
    public UnitDescriptionPanel UnitDescriptionPanel => unitDescriptionPanel;
    public int Score => score;
    public bool ExitActivated => _currentRoom.ExitActivated;
    public bool NeedToShowUnitsUI => UnitsQueue.IsUnitCurrent(_metaPlayer) 
                                     && !_metaPlayer.MovementAbility.PathToMoveIsSelected 
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
        _metaPlayer.AbleToDie.OnDeath.AddListener(() => loseScreen.SetActive(true));
    }

    public void StartNextRoom()
    {
        var newRoom = Instantiate(roomsPrefabs.GetRandomElement());
        _roomsCount++;
        _currentRoom.Deactivate();
        _currentRoom = newRoom;
        _currentRoom.Init(_metaPlayer);
        _metaPlayer.ActionPoints.SetPoints(8);
        _metaPlayer.Health.TakeHealing(33);
        waveSpawner.ResetRoundsTimer();
        waveSpawner.SpawnPreWave();
    }

    public void IncreaseScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();

        if (this.score - _hashedScoreThenExitActivated < deltaOfScoreToOpenExit)
            return;

        _currentRoom.ActivateExit();
        _hashedScoreThenExitActivated += deltaOfScoreToOpenExit;
    }

    public void SpawnUnit(Unit prefab, Unit spawner, Cell targetCell)
    {
        var spawnedUnit = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        spawnedUnit.Init();
        spawnedUnit.CurrentCell = spawner.CurrentCell;
        spawnedUnit.Movable.Move(targetCell, false);

        if (spawnedUnit.ActionsInput != null)
            UnitsQueue.AddObjectInQueue(spawnedUnit);
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
