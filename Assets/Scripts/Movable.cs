using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    public void Move(Unit unit, Cell targetCell)
    {
        if (!targetCell.isEmpty)
            return;

        unit.currentCell.Content = null;
        targetCell.Content = unit;
        unit.transform.position = targetCell.transform.position;        
    }

    public void Move(Cell targetCell)
    {
        Move(_unit, targetCell);
    }
}
