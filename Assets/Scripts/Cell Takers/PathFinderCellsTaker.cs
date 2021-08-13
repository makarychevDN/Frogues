using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderCellsTaker : BaseCellsTaker
{
    [SerializeField] private Unit _unit;
    [SerializeField] private IntContainer _currentActionPoints;
    public override List<Cell> Take()
    {
        return PathFinder.Instance.GetCellsAreaByActionPoints(_unit._currentCell, _currentActionPoints.Content);
    }
}
