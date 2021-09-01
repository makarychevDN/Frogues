using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizePath : BaseCellsEffect
{
    [SerializeField] private BaseCellsTaker _validCellsTaker;
    [SerializeField] private CellByMousePosition _choosedCellTaker;
    [SerializeField] private Unit _user;
    private Cell choosedCell;

    public override void ApplyEffect()
    {
        MapBasedOnTilemap.Instance._allCells.ForEach(cell => cell.EnablePathDot(false));

        if (_choosedCellTaker.Take() == null)
            return;
            
        choosedCell = _choosedCellTaker.Take()[0];
        if (choosedCell != null && _validCellsTaker.Take().Contains(choosedCell))
        {
            PathFinder.Instance.FindWay(_user._currentCell, choosedCell).ForEach(cell => cell.EnablePathDot(true));
        }
    }
}
