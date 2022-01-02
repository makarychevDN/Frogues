using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWayInValidCells : BaseCellsTaker
{
    [SerializeField] private BaseCellsTaker validCellsTaker;
    [SerializeField] private CellByMousePosition choosedCellTaker;
    [SerializeField] private Unit user;
    [SerializeField] private bool ignoreDefaultUnits, ignoreProjectiles, ignoreSurfaces;

    private Cell _choosedCell;

    public override List<Cell> Take()
    {
        if (choosedCellTaker.Take() == null)
            return null;
            
        _choosedCell = choosedCellTaker.Take()[0];
        if (validCellsTaker.Take().Contains(_choosedCell))
        {
            return PathFinder.Instance.FindWay(user.currentCell, _choosedCell, ignoreDefaultUnits, ignoreProjectiles, ignoreSurfaces);
        }

        return null;
    }
}
