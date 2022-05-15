using UnityEngine;

namespace FroguesFramework
{
    public class
        Level : MonoBehaviour //да сейчас это используется как заглушка для расставления юнитов по карте, потом оно будет более полезным
    {
        [SerializeField] private Unit _player;
        [SerializeField] private Unit enemy1;
        [SerializeField] private Unit enemy2;

        [SerializeField] private Unit enemy3;
        [SerializeField] private Unit bloodSurface;

        //[SerializeField] private Unit _enemie;
        [SerializeField] private Map _map;
        [SerializeField] private Vector2Int _startPos;

        private void Start()
        {
            _map.layers[MapLayer.DefaultUnit][2, 2].Content = _player;
            _map.layers[MapLayer.Surface][3, 2].Content = bloodSurface;
            _map.layers[MapLayer.DefaultUnit][2, 5].Content = enemy1;
            _map.layers[MapLayer.DefaultUnit][5, 2].Content = enemy2;
            _map.layers[MapLayer.DefaultUnit][5, 5].Content = enemy3;
            _map.layers[MapLayer.DefaultUnit][5, 5].Content = enemy3;
        }
    }
}
