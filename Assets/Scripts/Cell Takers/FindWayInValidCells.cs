using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWayInValidCells : BaseCellsTaker
{
    [SerializeField] private BaseCellsTaker _validCellsTaker;
    [SerializeField] private CellByMousePosition _choosedCellTaker;
    [SerializeField] private Unit _user;
    private Cell choosedCell;

    public override List<Cell> Take()
    {
        if (_choosedCellTaker.Take() == null)
            return null;
            
        choosedCell = _choosedCellTaker.Take()[0];
        if (_validCellsTaker.Take().Contains(choosedCell))
        {
            return PathFinder.Instance.FindWay(_user._currentCell, choosedCell);
        }

        return null;
    }
}
