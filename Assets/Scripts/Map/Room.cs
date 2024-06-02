using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private Cell startPlayerPosition;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;
        private HashSet<IAbleToDisablePreVisualization> ableToDisablePreVisualizationObjects = new();

        public Map Map => map;
        public PathFinder PathFinder => pathFinder;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;

        public void Init(List<Unit> playableCharacters)
        {
            List<Unit> otherAbleToACtCharacters = GetComponentsInChildren<Unit>().ToList();
            GetComponentsInChildren<Unit>().ToList().ForEach(unit => unit.Init());
            foreach (var unit in playableCharacters)
            {
                unit.Init();
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
            ableToDisablePreVisualizationObjects.Clear();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
