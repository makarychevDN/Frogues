using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private Cell startPlayerPosition;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;
        private List<IAbleToDisablePreVisualization> _ableToDisablePreVisualizationObjects = new();

        public Map Map => map;
        public PathFinder PathFinder => pathFinder;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;

        public UnityEvent OnSomeoneMoved;
        public UnityEvent OnSomeoneDied;

        public void Init(List<Unit> playableCharacters)
        {
            List<Unit> otherAbleToACtCharacters = GetComponentsInChildren<Unit>().ToList();
            GetComponentsInChildren<Unit>().ToList().ForEach(unit => unit.Init(this));
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
    }
}
