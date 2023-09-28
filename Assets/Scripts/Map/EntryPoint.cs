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
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private UnitDescriptionPanel unitDescriptionPanel;
    [SerializeField] private int score;
    [SerializeField] private int deltaOfScoreToOpenExit = 200;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private TMP_Text scoreText;
    private int _roomsCount;
    private int _hashedScoreThenExitActivated;
    private HashSet<IAbleToDisablePreVisualization> ableToDisablePreVisualizationObjects = new();

    public bool CurrentRoomIsHub => _currentRoom == hub;
    public CameraController CameraController => _currentRoom.CameraController;
    public PathFinder PathFinder => _currentRoom.PathFinder;
    public Map Map => _currentRoom.Map;
    public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;
    public bool PauseIsActive => pausePanel.activeSelf;
    public UnitDescriptionPanel UnitDescriptionPanel => unitDescriptionPanel;
    public int Score => score;
    public bool ExitActivated => exitButton.activeSelf;
    public bool NeedToShowUnitsUI => UnitsQueue.IsUnitCurrent(_metaPlayer) 
                                     && !_metaPlayer.MovementAbility.PathToMoveIsSelected 
                                     && !CurrentlyActiveObjects.SomethingIsActNow;

    public Vector3 CenterOfRoom => _currentRoom.CenterOfRoom;

    public void ActivateExit() => _currentRoom.ActivateExit();

    public Unit MetaPlayer => _metaPlayer;

    private void Awake()
    {
        Instance = this;
        _abilitiesPanel.Init();
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
        _metaPlayer.ActionPoints.SetCurrentPoints(4);
        _metaPlayer.Health.TakeHealing(33);
        waveSpawner.ResetRoundsTimer();
        waveSpawner.SpawnPreWave();
        exitButton.SetActive(false);
        FindObjectsOfType<MonoBehaviour>().OfType<IAbleToHaveCooldown>().ToList().ForEach(x => x.SetCooldownAsAfterStart());
    }

    public void IncreaseScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();

        if (this.score - _hashedScoreThenExitActivated < deltaOfScoreToOpenExit)
            return;

        //_currentRoom.ActivateExit();
        exitButton.SetActive(true);
        _hashedScoreThenExitActivated += deltaOfScoreToOpenExit;
    }

    public void SpawnUnit(Unit prefab, Unit spawner, Cell targetCell)
    {
        var spawnedUnit = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        spawnedUnit.CurrentCell = spawner.CurrentCell;
        spawnedUnit.Init();
        spawnedUnit.Movable.Move(targetCell, false);
        spawnedUnit.transform.parent = Map.transform;

        if (spawnedUnit.ActionsInput != null)
            UnitsQueue.AddObjectInQueue(spawnedUnit);
    }

    public void SpawnUnit(Unit prefab, Cell targetCell)
    {
        var spawnedUnit = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        spawnedUnit.CurrentCell = targetCell;
        spawnedUnit.Init();
        spawnedUnit.transform.position = targetCell.transform.position;
        spawnedUnit.transform.parent = Map.transform;

        if (prefab.SurfaceUnitExtension == null)
            targetCell.Content = spawnedUnit;
        else
            targetCell.Surfaces.Add(spawnedUnit);

        if (spawnedUnit.ActionsInput != null)
            UnitsQueue.AddObjectInQueue(spawnedUnit);
    }

    private void Update()
    {

        if (PauseIsActive)
            return;

        _currentRoom.UnitsQueue.ActForCurrentUnit();
    }

    public void AddAbleToDisablePreVisualizationToCollection(IAbleToDisablePreVisualization ableToDisablePreVisualization)
    {
        ableToDisablePreVisualizationObjects.Add(ableToDisablePreVisualization);
    }

    public void RemoveAbleToDisablePreVisualizationToCollection(IAbleToDisablePreVisualization ableToDisablePreVisualization)
    {
        ableToDisablePreVisualizationObjects.Remove(ableToDisablePreVisualization);
    }

    public void DisableAllPrevisualization()
    {
        foreach (var ableToDisablePreVisualization in ableToDisablePreVisualizationObjects)
        {
            ableToDisablePreVisualization.DisablePreVisualization();
        }
    }
}
