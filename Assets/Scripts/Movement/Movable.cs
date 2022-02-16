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
    public UnityEvent<List<Cell>> OnBumpIntoCell;

    private MovableAnimation _movableAnimation;

    private void Awake()
    {
        _movableAnimation = GetComponent<MovableAnimation>();
    }

    public void Move(Cell targetCell) => Move(targetCell, true);
    
    public void Move(Cell targetCell, bool startCellBecomeEmptyOnMove)
    {
        //if ((!targetCell.IsEmpty && (!targetCell.Content.small)/* || !canBumpIntoUnit)*/ || !IsActionPointsEnough()))
           //return;
           
        //if (!targetCell.IsEmpty && (!targetCell.Content.small) || !targetCell.IsEmpty && (!canBumpIntoUnit) || !IsActionPointsEnough()) //todo неработает блять
            //return;
        
        if (!targetCell.IsEmpty && !(canBumpIntoUnit || targetCell.Content.small) || !IsActionPointsEnough())
            return;

        SpendActionPoints();
        targetCell.chosenToMovement = true;
        
        if(startCellBecomeEmptyOnMove)
            unit.currentCell.Content = null;

        _movableAnimation.Play(unit.currentCell, targetCell);
        unit.currentCell = null;
        OnMovementStart.Invoke();
    }

    public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight) =>
        Move(targetCell, movementCost, speed, jumpHeight, true);
    
    public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight, bool startCellBecomeEmptyOnMove)
    {
        //if (!targetCell.IsEmpty && (!targetCell.Content.small)/* || !canBumpIntoUnit)*/ || !IsActionPointsEnough(movementCost)) //todo неработает блять
            //return;
            
        //if (!targetCell.IsEmpty && (!targetCell.Content.small) || !targetCell.IsEmpty && (!canBumpIntoUnit) || !IsActionPointsEnough(movementCost)) //todo неработает блять
        //    return;

        if (!targetCell.IsEmpty && !(canBumpIntoUnit || targetCell.Content.small) || !IsActionPointsEnough())
            return;

        SpendActionPoints(movementCost);
        targetCell.chosenToMovement = true;

        if (startCellBecomeEmptyOnMove)
            unit.currentCell.Content = null;

        _movableAnimation.Play(unit.currentCell, targetCell, speed, jumpHeight);
        unit.currentCell = null;
        OnMovementStart.Invoke();
    }

    public void StopMovement(Cell targetCell)
    {
        targetCell.chosenToMovement = false;
        unit.transform.position = targetCell.transform.position;
        
        if (!targetCell.IsEmpty)
        {
            if (!unit.small && targetCell.Content.small)
            {
                var unitToStepOnIt = targetCell.Content;
                targetCell.Content = unit;
                unitToStepOnIt.currentCell = null;
                unit.currentCell = targetCell;
                unitToStepOnIt.stepOnUnitTrigger.Run(unit);
                return;
            }
            
            OnBumpInto.Invoke();
            OnBumpIntoCell.Invoke(new List<Cell>() {targetCell});
            return;
        }
        
        targetCell.Content = unit;
        OnMovementEnd.Invoke();
    }
}
