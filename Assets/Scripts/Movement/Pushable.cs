using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Pushable : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField, ReadOnly] private Vector2IntContainer lastPushDirection;

    public UnityEvent OnPushed;
    
    public void Push(Cell pusherCell)
    {
        lastPushDirection.Content = CalculateMovementVector(pusherCell);
        OnPushed.Invoke();
    }

    private Vector2Int CalculateMovementVector(Cell pusherCell)
    {
        return (unit.Coordinates - pusherCell.coordinates).ToVector3().normalized.ToVector2Int();
    }
}