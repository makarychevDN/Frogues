using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ArenaInitializer : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private Unit player;
        [SerializeField] private Unit metaPlayer;
        
        private void Awake()
        {
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

        private void InitPlayer()
        {
            if (metaPlayer == null)
            {
                metaPlayer = player;
            }
            
            var playerInstance = metaPlayer;
            player.currentCell.Content = playerInstance;
            playerInstance.currentCell = player.currentCell;
            playerInstance.transform.position = player.transform.position;
            player.gameObject.SetActive(false);
            player = playerInstance;

            var actionPoints = player.GetComponentInChildren<ActionPoints>();
            actionPoints.CurrentActionPoints = actionPoints.RegenActionPoints;
        }
    }
}
