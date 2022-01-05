using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour //да сейчас это используется как заглушка для расставления юнитов по карте, потом оно будет более полезным
{
    [SerializeField] private Unit _player;
    [SerializeField] private Unit _projectile;
    //[SerializeField] private Unit _enemie;
    [SerializeField] private Map _map;
    [SerializeField] private Vector2Int _startPos;

    private void Start()
    {
        _map.unitsLayer[3, 3].Content = _player;
        //_map.unitsLayer[2, 1].Content = _enemie;

        if(_projectile != null)
            _map.projectilesLayer[4, 1].Content = _projectile;
    }
}
