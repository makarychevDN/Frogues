using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovableAnimation))]
public class Movable : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    //private UnityEvent<Cell, Cell> OnMoveForAnimation = new UnityEvent<Cell, Cell>();
    private MovableAnimation _movableAnimation;

    private void Start()
    {
        _movableAnimation = GetComponent<MovableAnimation>();
    }

    public void Move(Unit unit, Cell targetCell)
    {
        if (!targetCell.isEmpty)
            return;

        _movableAnimation.Play(unit.currentCell, targetCell);

        unit.currentCell.Content = null;
        targetCell.Content = unit;
                
    }

    public void Move(Cell targetCell)
    {
        Move(_unit, targetCell);
    }

    public void UpdatePosition(Cell targetCell)
    {
        _unit.transform.position = targetCell.transform.position;
    }
}
