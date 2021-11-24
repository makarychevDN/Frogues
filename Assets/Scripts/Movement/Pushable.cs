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
    public UnityEvent OnPrepushed;
    
    public void Push(Cell pusherCell)
    {
        lastPushDirection.Content = CalculateMovementVector(pusherCell);
        OnPushed.Invoke();
    }

    public void PrePush(Cell pusherCell)
    {
        lastPushDirection.Content = CalculateMovementVector(pusherCell);
        OnPrepushed.Invoke();
        print("Daaamn u wanna push me");
    }

    private Vector2Int CalculateMovementVector(Cell pusherCell)
    {
        return (unit.Coordinates - pusherCell.coordinates).ToVector3().normalized.ToVector2Int();
    }
}
