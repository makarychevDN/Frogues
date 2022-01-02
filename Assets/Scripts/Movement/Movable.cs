using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovableAnimation))]
public class Movable : CostsActionPointsBehaviour
{
    [Space]
    [SerializeField] private Unit unit;
    public UnityEvent OnMovementStart;
    public UnityEvent OnMovementEnd;
    private MovableAnimation _movableAnimation;

    private void Awake()
    {
        _movableAnimation = GetComponent<MovableAnimation>();
    }

    public void Move(Cell targetCell)
    {
        if (!targetCell.IsEmpty || !IsActionPointsEnough())
            return;

        SpendActionPoints();
        targetCell.chosenToMovement = true;
        unit.currentCell.Content = null;

        _movableAnimation.Play(unit.currentCell, targetCell);
        OnMovementStart.Invoke();
    }
    
    public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight)
    {
        if (!targetCell.IsEmpty || !IsActionPointsEnough(movementCost))
            return;

        SpendActionPoints(movementCost);
        targetCell.chosenToMovement = true;
        unit.currentCell.Content = null;

        _movableAnimation.Play(unit.currentCell, targetCell, speed, jumpHeight);
        OnMovementStart.Invoke();
    }

    public void StopMovement(Cell targetCell)
    {
        unit.transform.position = targetCell.transform.position;
        targetCell.Content = unit;
        OnMovementEnd.Invoke();
        targetCell.chosenToMovement = false;
    }
}
