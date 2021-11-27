using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllEmptyCellsTaker : BaseCellsTaker
{
    public override List<Cell> Take()
    {
        var cells = Map.Instance.allCells;
        return cells.Where(cell => cell.ColumnIsEmpty()).ToList();
    }
}
