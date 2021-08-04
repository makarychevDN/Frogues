using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private MapLayer _unitType;
    public Movable _movable;
    public Cell _currentCell;

    public Vector2Int Coordinates => _currentCell._coordinates;
}

