using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [field: Header("Images")]
        [field: SerializeField] public Sprite AvailableSprite { get; set; }
        [field: SerializeField] public Sprite UnavailableSprite { get; set; }

        [Header("Setup")]
        [SerializeField] private Map map;
        [SerializeField] private Cell startPlayerPosition;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private CurrentlyActiveObjects currentlyActiveObjects;
        [SerializeField] private List<Unit> playableCharacters;
        [SerializeField] private List<Unit> bloodPuddles;

        [Header("Mechanics")]
        [SerializeField] private int ratsInTheRoomCount;

        private List<IAbleToDisablePreVisualization> _ableToDisablePreVisualizationObjects = new();
        private bool _wasInitedAlready;

        public Map Map => map;
        public bool NeedToShowUnitsUI => true;
        public PathFinder PathFinder => pathFinder;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;
        public CurrentlyActiveObjects CurrentlyActiveObjects => currentlyActiveObjects;
        public List<Unit> PlayableCharacters => playableCharacters;
        public int RatsInTheRoomCount { get => ratsInTheRoomCount; set { ratsInTheRoomCount = value; OnCountOfRatsUpdated.Invoke(ratsInTheRoomCount); } }
        public int BloodPuddlesInTheRoomCount => bloodPuddles.Count;

        public UnityEvent OnSomeoneMoved;
        public UnityEvent OnSomeoneDied;
        public UnityEvent<int> OnCountOfRatsUpdated;
        public UnityEvent<int> OnCountOfBloodPuddlesUpdated;
        public UnityEvent<bool> OnRoomWasEnabled;

        public void Init(List<Unit> playableCharacters)
        {
            this.playableCharacters = playableCharacters;
            GetComponentsInChildren<Unit>().ToList().ForEach(unit => unit.Init(this));
            List<Unit> otherAbleToACtCharacters = GetComponentsInChildren<Unit>().Where(unit => unit.ActionsInput != null).ToList();
            foreach (var unit in playableCharacters)
            {
                unit.Init(this);
                var targetCell = map.allCells.EmptyCellsOnly().GetRandomElement();
                unit.CurrentCell = targetCell;
                targetCell.Content = unit;
                unit.transform.position = targetCell.transform.position;
            }

            cameraController.Init(this);
            OnRoomWasEnabled.Invoke(true);

            if (_wasInitedAlready)
                return; 
            
            _wasInitedAlready = true;
            map.Init();
            pathFinder.Init();
            unitsQueue.Init(this, playableCharacters, otherAbleToACtCharacters);
        }

        public void UnInit(List<Unit> playableCharacters)
        {
            cameraController.Deactivate();

            foreach (var unit in playableCharacters)
            {
                unit.CurrentCell.Content = null;
            }

            OnRoomWasEnabled.Invoke(false);
        }

        public void DisableAllPrevisualization()
        {
            _ableToDisablePreVisualizationObjects.ForEach(previsualization => previsualization.DisablePreVisualization());
        }

        public void AddAbleToDisablePrevisualizationObject(IAbleToDisablePreVisualization ableToDisablePreVisualization)
        {
            _ableToDisablePreVisualizationObjects.Add(ableToDisablePreVisualization);
        }

        public void RemoveAbleToDisablePrevisualizationObject(IAbleToDisablePreVisualization ableToDisablePreVisualization)
        {
            _ableToDisablePreVisualizationObjects.Remove(ableToDisablePreVisualization);
        }

        public void AddAbleToDisablePrevisualizationObjects(List<IAbleToDisablePreVisualization> ableToDisablePreVisualizationObjects)
        {
            _ableToDisablePreVisualizationObjects.AddRange(ableToDisablePreVisualizationObjects);
        }

        public void RemoveAbleToDisablePrevisualizationObjects(List<IAbleToDisablePreVisualization> ableToDisablePreVisualizationObjects)
        {
            _ableToDisablePreVisualizationObjects = _ableToDisablePreVisualizationObjects.Except(ableToDisablePreVisualizationObjects).ToList();
        }

        public void InvokeOnSomeoneMoved()
        {
            OnSomeoneMoved.Invoke();
        }

        public void InvokeOnSomeoneDied()
        {
            OnSomeoneDied.Invoke();
        }

        private void Update()
        {
            unitsQueue.ActForCurrentUnit();
        }

        public void AddBloodPuddle(Unit bloodPuddle)
        {
            bloodPuddles.Add(bloodPuddle);
            OnCountOfBloodPuddlesUpdated.Invoke(bloodPuddles.Count);
        }

        public void RemoveBloodPuddle(Unit bloodPuddle)
        {
            bloodPuddles.Remove(bloodPuddle);
            OnCountOfBloodPuddlesUpdated.Invoke(bloodPuddles.Count);
        }
    }
}
