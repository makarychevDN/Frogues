    using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private Cell exit;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraRotationPointController centerPointForCamera;

        [SerializeField] private Unit player;
        [SerializeField] private Unit metaPlayer;

        public PathFinder PathFinder => pathFinder;
        public Map Map => map;
        public UnitsQueue UnitsQueue => unitsQueue;
        
        public Vector3 CenterOfRoom => centerPointForCamera.transform.position;

        public void Init()
        {
            centerPointForCamera.Init();
            map.Init();
            pathFinder.Init();
            InitPlayer();
            unitsQueue.Player = player;

            if(exit != null)
                exit.gameObject.SetActive(false);

            foreach (var ableToAct in FindObjectsOfType<MonoBehaviour>().OfType<IAbleToAct>())
            {
                ableToAct.Init();
            }
            
            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.Init();
            }
            
            unitsQueue.Init();
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
            centerPointForCamera.Deactivate();
            gameObject.SetActive(false);
        }
    }
}
