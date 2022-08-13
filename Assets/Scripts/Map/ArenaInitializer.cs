using UnityEngine;

namespace FroguesFramework
{
    public class ArenaInitializer : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private Unit player;

        private void Awake()
        {
            map.Init();
            pathFinder.Init();
            InitPlayer();
            unitsQueue.Player = player;
            unitsQueue.Init();
        }

        private void InitPlayer()
        {
            var playerInstanceContainer = FindObjectOfType<PlayerInstanceContainer>();
            
            if (playerInstanceContainer.Content == null)
            {
                playerInstanceContainer.Content = player;
            }
            else
            {
                
                playerInstanceContainer.Content.currentCell = player.currentCell;
                playerInstanceContainer.Content.transform.position = player.transform.position;
                player.gameObject.SetActive(false);
                player = playerInstanceContainer.Content;
            }
        }
    }
}
