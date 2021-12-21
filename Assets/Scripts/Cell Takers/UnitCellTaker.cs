using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCellTaker : BaseCellsTaker
{
    [SerializeField] private Unit unit;

    public override List<Cell> Take() => new List<Cell>() { unit.currentCell };
}
