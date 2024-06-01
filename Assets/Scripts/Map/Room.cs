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

        public Map Map => map;
        public PathFinder PathFinder => pathFinder;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;

        public void Init(List<Unit> playableCharacters)
        {
            playableCharacters.ForEach(unit => unit.Init());

            foreach(var unit in playableCharacters)
            {
                unit.Init();
                unit.Movable.Move(map.allCells.GetRandomElement(), startCellBecomeEmptyOnMove: false, needToModificateJumpHeightByDistance: false);
            }
            GetComponentsInChildren<Unit>().ToList().ForEach(unit => unit.Init());
            cameraController.Init();
            map.Init();
            pathFinder.Init();

            foreach (var unit in GetComponentsInChildren<Unit>())
            {
                unit.Init();
            }

            var ableToActObjects = GetComponentsInChildren<MonoBehaviour>().OfType<IAbleToAct>();
            foreach (var ableToAct in ableToActObjects)
            {
                ableToAct.Init();
            }

            unitsQueue.Init(playableCharacters);
        }

        public void Deactivate()
        {
            cameraController.Deactivate();
            GetComponentsInChildren<Cell>().ToList().ForEach(cell => EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(cell));
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
