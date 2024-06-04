using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Map map;
        [SerializeField] private Cell startPlayerPosition;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;

        [Header("Mechanics")]
        [SerializeField] private int ratsInTheRoomCount;
        [SerializeField] private int bloodPuddlesInTheRoomCount;

        private List<IAbleToDisablePreVisualization> _ableToDisablePreVisualizationObjects = new();

        public Map Map => map;
        public PathFinder PathFinder => pathFinder;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;
        public int RatsInTheRoomCount { get => ratsInTheRoomCount; set { ratsInTheRoomCount = value; OnCountOfRatsUpdated.Invoke(ratsInTheRoomCount); } }
        public int BloodPuddlesInTheRoomCount { get => bloodPuddlesInTheRoomCount; set { bloodPuddlesInTheRoomCount = value; OnCountOfBloodPuddlesUpdated.Invoke(ratsInTheRoomCount); } }

        public UnityEvent OnSomeoneMoved;
        public UnityEvent OnSomeoneDied;
        public UnityEvent<int> OnCountOfRatsUpdated;
        public UnityEvent<int> OnCountOfBloodPuddlesUpdated;

        public void Init(List<Unit> playableCharacters)
        {
            GetComponentsInChildren<Unit>().ToList().ForEach(unit => unit.Init(this));
            List<Unit> otherAbleToACtCharacters = GetComponentsInChildren<Unit>().Where(unit => unit.ActionsInput != null).ToList();
            foreach (var unit in playableCharacters)
            {
                unit.Init(this);
                unit.Movable.Move(map.allCells.EmptyCellsOnly().GetRandomElement(), startCellBecomeEmptyOnMove: false, needToModificateJumpHeightByDistance: false);
            }

            cameraController.Init(this);
            map.Init();
            pathFinder.Init();
            unitsQueue.Init(playableCharacters, otherAbleToACtCharacters);
        }

        public void Deactivate()
        {
            cameraController.Deactivate();
            _ableToDisablePreVisualizationObjects.Clear();
            gameObject.SetActive(false);
            //Destroy(gameObject);
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
    }
}
