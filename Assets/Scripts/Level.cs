using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Unit _player;
    [SerializeField] private Unit _projectile;
    [SerializeField] private Unit _enemie;
    [SerializeField] private MapBasedOnTilemap _map;
    [SerializeField] private Vector2Int _startPos;

    private void Start()
    {
        _map.unitsLayer[1, 1].Content = _player;
        _map.unitsLayer[2, 1].Content = _enemie;
        _map.projectilesLayer[4, 1].Content = _projectile;
    }
}
