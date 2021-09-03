using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovableAnimation))]
public class Movable : CostsActionPointsBehaviour
{
    [SerializeField] private Unit unit;
    public UnityEvent OnMovementStart;
    public UnityEvent OnMovementEnd;
    private MovableAnimation _movableAnimation;

    private void Start()
    {
        _movableAnimation = GetComponent<MovableAnimation>();
    }

    public void Move(Cell targetCell)
    {
        if (!targetCell.IsEmpty || !IsActionPointsEnough())
            return;

        SpendActionPoints();
        unit._currentCell.Content = null;
        _movableAnimation.Play(unit._currentCell, targetCell);
        OnMovementStart.Invoke();
    }

    public void StopMovement(Cell targetCell)
    {
        unit.transform.position = targetCell.transform.position;
        targetCell.Content = unit;
        OnMovementEnd.Invoke();
    }
}
