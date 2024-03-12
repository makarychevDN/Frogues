using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static FroguesFramework.RewardsGenerator;

namespace FroguesFramework
{
    public class EntryPoint : MonoBehaviour, IRoundTickable
    {
        public static EntryPoint Instance;
        [SerializeField] private Room hub;
        [SerializeField] private AscensionSetup ascensionSetup;
        [SerializeField] private List<Room> roomsPrefabs;
        [SerializeField] private Room _currentRoom;
        [SerializeField] private Unit _metaPlayer;
        [SerializeField] private AbilitiesPanel _abilitiesPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private BonfirePanel bonfirePanel;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private GameObject exitButton;
        [SerializeField] private GameObject endTurnButton;
        [SerializeField] private UnitDescriptionPanel unitDescriptionPanel;
        [SerializeField] private AbilityHint abilityHint;
        [SerializeField] private int score;
        [SerializeField] private int scoreRequiredToStartFinalPart = 1500;
        [SerializeField] private int campfiresAfterFinalScoreCountRequiredToWin = 2;
        [SerializeField] private int campfiresAfterFinalScoreCount;
        [SerializeField] private WavesGenerator wavesGenerator;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private float bonfireHealingValueMultiplier = 1;
        [SerializeField] private int turnCounter;
        [SerializeField] private ResourcePointsUI playersActionPointsUI;
        [SerializeField] private ResourcePointsUI playersBloodPointsUI;
        [SerializeField] private AnimationCurve defaultMovementCurve;
        [SerializeField] private int countOfRats = 0;
        [Header("Abilities Setup")]
        [SerializedDictionary("Type Of Abilities List", "Abilities List")]
        [SerializeField] private SerializedDictionary<RewardType, List<BaseAbility>> possibleRewards;

        private int _roomsCount;
        private int _scoreDeltaCounter;
        private List<Unit> _bloodSurfacesInCurrentRoom = new();
        private HashSet<IAbleToDisablePreVisualization> ableToDisablePreVisualizationObjects = new();
        public UnityEvent OnNextRoomStarted;
        public UnityEvent OnSomeoneMoved;
        public UnityEvent OnSomeoneDied;
        public UnityEvent OnScoreIncreased;
        public UnityEvent OnBloodSurfacesCountOnTheMapUpdated;
        public UnityEvent<int> OnCountOfRatsUpdated;
        public UnityEvent OnFinalPartStarted;
        public UnityEvent OnWin;

        public bool CurrentRoomIsPeaceful => _currentRoom.IsPeaceful;
        public SerializedDictionary<RewardType, List<BaseAbility>> PossibleRewards => possibleRewards;
        public CameraController CameraController => _currentRoom.CameraController;
        public PathFinder PathFinder => _currentRoom.PathFinder;
        public Map Map => _currentRoom.Map;
        public UnitsQueue UnitsQueue => _currentRoom.UnitsQueue;
        public bool PauseIsActive => pausePanel.activeSelf || unitDescriptionPanel.IsActive;
        public UnitDescriptionPanel UnitDescriptionPanel => unitDescriptionPanel;
        public AbilityHint AbilityHint => abilityHint;
        public int Score => score;
        public float BonfireHealingMultiplierValue => bonfireHealingValueMultiplier;
        public bool ExitActivated => exitButton.activeSelf;
        public GameObject EndTurnButton => endTurnButton;
        public AscensionSetup AscensionSetup => ascensionSetup;
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

            if(CurrentAscention.ascensionSetup != null)
                ascensionSetup = CurrentAscention.ascensionSetup;

            _abilitiesPanel.Init(_metaPlayer);
            _currentRoom = hub;
            _currentRoom.Init(_metaPlayer);
            _metaPlayer.AbleToDie.OnDeath.AddListener(() => loseScreen.SetActive(true));
            _metaPlayer.AbleToDie.OnDeath.AddListener(() => CurrentlyActiveObjects.Clear());
            playersActionPointsUI.Init(_metaPlayer.ActionPoints);
            playersBloodPointsUI.Init(_metaPlayer.BloodPoints);
            turnCounter = 1;
            bonfirePanel.Init();
        }

        public void SetAscensionSetup(AscensionSetup setup)
        {
            ascensionSetup = setup;
        }

        public void StartNextRoom()
        {
            countOfRats = 0;
            turnCounter = 1;
            var newRoom = Instantiate(roomsPrefabs[_roomsCount]);
            _roomsCount++;

            if(_roomsCount >= roomsPrefabs.Count)
                _roomsCount = 0;

            _scoreDeltaCounter = 0;
            _currentRoom.Deactivate();
            _currentRoom = newRoom;
            _currentRoom.Init(_metaPlayer);
            _metaPlayer.ActionPoints.SetCurrentPoints(4);
            _metaPlayer.Stats.RemoveAllNonConstantlyEffects();
            _metaPlayer.Health.RemoveAllBlockEffects();
            wavesGenerator.ResetRoundsTimer();
            exitButton.SetActive(false);
            FindObjectsOfType<MonoBehaviour>().OfType<IAbleToHaveCooldown>().ToList().ForEach(x => x.SetCooldownAsAfterStart());
            OnNextRoomStarted.Invoke();
            _bloodSurfacesInCurrentRoom.Clear();
            OnBloodSurfacesCountOnTheMapUpdated.Invoke();

            if (!CurrentRoomIsPeaceful)
                wavesGenerator.SpawnEnemies();
        }

        public void TryToCountCampfireAfterFinalPartStarted()
        {
            if (score < scoreRequiredToStartFinalPart)
                return;

            campfiresAfterFinalScoreCount++;

            if(campfiresAfterFinalScoreCount == 1)
                OnFinalPartStarted.Invoke();

            if (campfiresAfterFinalScoreCount >= campfiresAfterFinalScoreCountRequiredToWin)
                OnWin.Invoke();
        }

        public void IncreaseBonfireHealingValue(float value) => bonfireHealingValueMultiplier += value;

        public void IncreaseScore(int score, bool resetDeltaValue = false)
        {
            this.score += score;
            _scoreDeltaCounter += score;
            scoreText.text = this.score.ToString();
            OnScoreIncreased.Invoke();

            if (resetDeltaValue)
            {
                _scoreDeltaCounter = 0;
            }

            if (_scoreDeltaCounter < ascensionSetup.RequaredDeltaOfScoreToOpenExitToCampfire)
                return;

            exitButton.SetActive(true);
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

        public void EnableBonfireRestPanel(bool value)
        {
            bonfirePanel.gameObject.SetActive(value);
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

        public void InvokeSomeoneDied()
        {
            OnSomeoneDied.Invoke();
        }

        public void TickAfterEnemiesTurn() => turnCounter++;

        public void TickAfterPlayerTurn(){}
    }
}