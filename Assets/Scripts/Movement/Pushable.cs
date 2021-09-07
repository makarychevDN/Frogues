using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pushable : MonoBehaviour
{
    [SerializeField] private Unit unit;    
    [SerializeField] private Movable movable;

    public UnityEvent OnPushed;
    
    public void Push(Cell pusherCell)
    {
        MapBasedOnTilemap.Instance.FindNeigborhoodForCell(unit.currentCell, CalculateMovementVector(pusherCell));
    }

    private Vector2Int CalculateMovementVector(Cell pusherCell)
    {
        return (unit.Coordinates - pusherCell.coordinates).ToVector3().normalized.ToVector2Int();
    }
}
