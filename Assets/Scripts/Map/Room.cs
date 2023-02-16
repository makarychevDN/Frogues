using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraRotationPointController centerPointForCamera;

        [SerializeField] private Unit player;
        [SerializeField] private Unit metaPlayer;

        public PathFinder PathFinder => pathFinder;
        public Map Map => map;
        public UnitsQueue UnitsQueue => unitsQueue;

        public void Init()
        {
            centerPointForCamera.Init();
            map.Init();
            pathFinder.Init();
            InitPlayer();
            unitsQueue.Player = player;
            
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

            //var actionPoints = player.GetComponentInChildren<ActionPoints>();
            //actionPoints.CurrentActionPoints = actionPoints.RegenActionPoints;
        }

        public void Deactivate()
        {
            centerPointForCamera.Deactivate();
            gameObject.SetActive(false);
        }
    }
}
