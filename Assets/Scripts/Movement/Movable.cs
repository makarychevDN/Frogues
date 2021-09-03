using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovableAnimation))]
public class Movable : CostsActionPointsBehaviour
{
    [SerializeField] private Unit _unit;
    private MovableAnimation _movableAnimation;

    public UnityEvent OnMovementStart;
    public UnityEvent OnMovementEnd;

    private void Start()
    {
        _movableAnimation = GetComponent<MovableAnimation>();
    }

    public void Move(Cell targetCell)
    {
        if (!targetCell.isEmpty || !IsActionPointsEnough())
            return;

        SpendActionPoints();
        _unit._currentCell.Content = null;
        _movableAnimation.Play(_unit._currentCell, targetCell);
        OnMovementStart.Invoke();
    }

    public void StopMovement(Cell targetCell)
    {
        _unit.transform.position = targetCell.transform.position;
        targetCell.Content = _unit;
        OnMovementEnd.Invoke();
    }
}