using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Unit _player;
    [SerializeField] private Unit _projectile;
    [SerializeField] private MapBasedOnTilemap _map;
    [SerializeField] private Vector2Int _startPos;

    private void Start()
    {
        _map._unitsLayer[0, 0].Content = _player;
        _map._projectilesLayer[3, 0].Content = _projectile;
        _player._movable.Move(_map._unitsLayer[0, 0]);
    }
}
