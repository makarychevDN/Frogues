using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseCellsEffect : MonoBehaviour
{
    public abstract void ApplyEffect();
    public abstract void ApplyEffect(List<Cell> cells);

    protected List<Cell> CellsListToCulumnsList(List<Cell> cells)
    {
        var columns = new List<Cell>();
        cells.ForEach(cell => Map.Instance.GetCellsColumn(cell.coordinates).ForEach(columnCell => columns.Add(columnCell)));
        return columns;
    }
}
