using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;

        public Map Map => map;
        public PathFinder PathFinder => pathFinder;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;

        public void Init()
        {
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

            unitsQueue.Init();
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
