using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MapLayer _unitType;
    public Movable _movable;
    public Cell _currentCell;
    public BaseInput _input;

    public Vector2Int Coordinates => _currentCell._coordinates;
}

