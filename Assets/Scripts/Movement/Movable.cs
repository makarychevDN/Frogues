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
    [Space] 
    public bool canBumpIntoUnit;
    public UnityEvent OnBumpInto;
    private MovableAnimation _movableAnimation;

    private void Awake()
    {
        _movableAnimation = GetComponent<MovableAnimation>();
    }

    public void Move(Cell targetCell)
    {
        if ((!targetCell.IsEmpty && !canBumpIntoUnit) || !IsActionPointsEnough())
            return;

        SpendActionPoints();
        targetCell.chosenToMovement = true;
        unit.currentCell.Content = null;

        _movableAnimation.Play(unit.currentCell, targetCell);
        OnMovementStart.Invoke();
    }
    
    public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight)
    {
        if ((!targetCell.IsEmpty && !canBumpIntoUnit) || !IsActionPointsEnough(movementCost))
            return;

        SpendActionPoints(movementCost);
        targetCell.chosenToMovement = true;
        unit.currentCell.Content = null;

        _movableAnimation.Play(unit.currentCell, targetCell, speed, jumpHeight);
        OnMovementStart.Invoke();
    }

    public void StopMovement(Cell targetCell)
    {
        targetCell.chosenToMovement = false;
        unit.transform.position = targetCell.transform.position;
        
        if (!targetCell.IsEmpty)
        {
            OnBumpInto.Invoke();
            return;
        }
        
        targetCell.Content = unit;
        OnMovementEnd.Invoke();
    }
}
