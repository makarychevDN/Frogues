using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class EntryPoint : MonoBehaviour, IRoundTickable
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
        [SerializeField] private AbilityHint abilityHint;
        [SerializeField] private int score;
        [SerializeField] private int deltaOfScoreToOpenExit = 200;
        [SerializeField] private WavesGenerator wavesGenerator;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private int bonfireHealingValue;
        [SerializeField] private int turnCounter;
        [SerializeField] private ResourcePointsUI playersActionPointsUI;
        [SerializeField] private ResourcePointsUI playersBloodPointsUI;
        [SerializeField] private AnimationCurve defaultMovementCurve;
        [SerializeField] private int countOfRats = 0;
        [Header("Abilities Setup")]
        [SerializeField] private List<BaseAbility> poolOfActiveAbilitiesPerRun;
        [SerializeField] private List<BaseAbility> poolOfPassiveAbilitiesPerRun;

        private int _roomsCount;
        private int _hashedScoreThenExitActivated;
        private List<Unit> _bloodSurfacesInCurrentRoom = new();
        private HashSet<IAbleToDisablePreVisualization> ableToDisablePreVisualizationObjects = new();
        public UnityEvent OnNextRoomStarted;
        public UnityEvent OnSomeoneMoved;
        public UnityEvent OnScoreIncreased;
        public UnityEvent OnBloodSurfacesCountOnTheMapUpdated;
        public UnityEvent<int> OnCountOfRatsUpdated;

        public bool CurrentRoomIsHub => _currentRoom == hub;
        public List<BaseAbility> PoolOfActiveAbilitiesPerRun => poolOfActiveAbilitiesPerRun;
        public List<BaseAbility> PoolOfPassiveAbilitiesPerRun => poolOfPassiveAbilitiesPerRun;
        public CameraController CameraController => _currentRoom.CameraController;
        public PathFinder PathFinder => _currentRoom.PathFinder;
        public Map Map => _currentRoom.Map;
        public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;
        public bool PauseIsActive => pausePanel.activeSelf || unitDescriptionPanel.IsActive;
        public UnitDescriptionPanel UnitDescriptionPanel => unitDescriptionPanel;
        public AbilityHint AbilityHint => abilityHint;
        public int Score => score;
        public int BonfireHealingValue => bonfireHealingValue;
        public bool ExitActivated => exitButton.activeSelf;
        public AnimationCurve DefaultMovementCurve => defaultMovementCurve;
        public bool NeedToShowUnitsUI => UnitsQueue.IsUnitCurrent(_metaPlayer)
                                         && !_metaPlayer.MovementAbility.PathToMoveIsSelected
                                         && !CurrentlyActiveObjects.SomethingIsActNow;

        public Vector3 CenterOfRoom => _currentRoom.CenterOfRoom;
        public void ActivateExit() => _currentRoom.ActivateExit();
        public Unit MetaPlayer => _metaPlayer;
        public int TurnCounter => turnCounter;
        public int CountOfRats 
        { 
            get => countOfRats; 

            set 
            {
                countOfRats = value;
                OnCountOfRatsUpdated.Invoke(value);
            } 
        }

        private void Awake()
        {
            Instance = this;
            _abilitiesPanel.Init(_metaPlayer);
            _currentRoom = hub;
            _currentRoom.Init(_metaPlayer);
            _metaPlayer.AbleToDie.OnDeath.AddListener(() => loseScreen.SetActive(true));
            playersActionPointsUI.Init(_metaPlayer.ActionPoints);
            playersBloodPointsUI.Init(_metaPlayer.BloodPoints);
            turnCounter = 1;
        }

        public void StartNextRoom()
        {
            countOfRats = 0;
            turnCounter = 1;
            var newRoom = Instantiate(roomsPrefabs.GetRandomElement());
            _roomsCount++;
            _currentRoom.Deactivate();
            _currentRoom = newRoom;
            _currentRoom.Init(_metaPlayer);
            _metaPlayer.ActionPoints.SetCurrentPoints(4);
            //_metaPlayer.Health.TakeHealing(33);
            _metaPlayer.Stats.RemoveAllNonConstantlyEffects();
            _metaPlayer.Health.RemoveAllBlockEffects();
            //waveSpawner.ResetRoundsTimer();
            //waveSpawner.SpawnPreWave();
            wavesGenerator.ResetRoundsTimer();
            exitButton.SetActive(false);
            FindObjectsOfType<MonoBehaviour>().OfType<IAbleToHaveCooldown>().ToList().ForEach(x => x.SetCooldownAsAfterStart());
            OnNextRoomStarted.Invoke();
            _bloodSurfacesInCurrentRoom.Clear();
            OnBloodSurfacesCountOnTheMapUpdated.Invoke();
        }

        public void IncreaseBonfireHealingValue(int value) => bonfireHealingValue += value;

        public void IncreaseScore(int score)
        {
            this.score += score;
            scoreText.text = this.score.ToString();
            OnScoreIncreased.Invoke();

            if (this.score - _hashedScoreThenExitActivated < deltaOfScoreToOpenExit)
                return;

            exitButton.SetActive(true);
            _hashedScoreThenExitActivated += deltaOfScoreToOpenExit;
        }

        public Unit SpawnUnit(Unit prefab, Unit spawner, Cell targetCell)
        {
            var spawnedUnit = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
            spawnedUnit.CurrentCell = spawner.CurrentCell;
            spawnedUnit.Init();
            spawnedUnit.Movable.Move(targetCell, false);
            spawnedUnit.transform.parent = Map.transform;

            if (spawnedUnit.ActionsInput != null)
                UnitsQueue.AddObjectInQueue(spawnedUnit);

            return spawnedUnit;
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

        public void AddBloodSurface(Unit bloodSurface)
        {
            _bloodSurfacesInCurrentRoom.Add(bloodSurface);
            OnBloodSurfacesCountOnTheMapUpdated.Invoke();
        }

        public void RemoveBloodSurface(Unit bloodSurface)
        {
            _bloodSurfacesInCurrentRoom.Remove(bloodSurface);
            OnBloodSurfacesCountOnTheMapUpdated.Invoke();
        }

        public int BloodSurfacesCount => _bloodSurfacesInCurrentRoom.Count;

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

        public void InvokeSomeoneMoved()
        {
            OnSomeoneMoved.Invoke();
        }

        public void TickAfterEnemiesTurn() => turnCounter++;

        public void TickAfterPlayerTurn(){}
    }
}