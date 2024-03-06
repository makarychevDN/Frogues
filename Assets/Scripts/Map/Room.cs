using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private bool isPeaceful;
        [SerializeField] private Cell exit;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;

        [SerializeField] private Unit player;
        [SerializeField] private Unit metaPlayer;

        [SerializeField] private BaseTrainingModificator trainingModificator;

        public PathFinder PathFinder => pathFinder;
        public Map Map => map;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;
        public bool IsPeaceful => isPeaceful;
        
        public Vector3 CenterOfRoom => cameraController.transform.position;
        public UnityEvent onRoomInited;

        public void Init()
        {
            cameraController.Init();
            map.Init();
            pathFinder.Init();
            InitPlayer();
            unitsQueue.Player = player;

            if(exit != null)
                exit.OnBecameFullByUnit.AddListener(TryToActivateNextRoom);

            foreach (var ableToAct in FindObjectsOfType<MonoBehaviour>().OfType<IAbleToAct>())
            {
                ableToAct.Init();
            }
            
            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.Init();
            }
            
            unitsQueue.Init();

            if(trainingModificator != null)
                trainingModificator.Init();

            onRoomInited.Invoke();
        }
        
        public void Init(Unit metaPlayer)
        {
            this.metaPlayer = metaPlayer;
            Init();
        }

        private void InitPlayer()
        {
            if (metaPlayer == null)
            {
                metaPlayer = player;
            }
            
            var playerInstance = metaPlayer;
            player.CurrentCell.Content = playerInstance;
            playerInstance.CurrentCell = player.CurrentCell;
            playerInstance.transform.position = player.transform.position;
            player.gameObject.SetActive(false);
            player = playerInstance;
        }

        public void ActivateExit()
        {
            if (exit.gameObject.activeSelf)
                return;

            exit.gameObject.SetActive(true);
            exit.Content = null;
            exit.OnBecameFullByUnit.AddListener(TryToActivateNextRoom);
        }

        private void TryToActivateNextRoom(Unit unit)
        {
            if(unit == metaPlayer)
            {
                EntryPoint.Instance.StartNextRoom();
            }
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
