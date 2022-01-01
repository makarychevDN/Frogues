using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitColumnCellTaker : BaseCellsTaker
{
    [SerializeField] private Unit unit;
    [SerializeField] private bool ignoreMySelf = true;

    public override List<Cell> Take() 
    { 
        var cells = Map.Instance.GetCellsColumn(unit.Coordinates);
        if (ignoreMySelf)
            cells.Remove(unit.currentCell);
        return cells;
    }
}
