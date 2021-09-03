using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderCellsTaker : BaseCellsTaker
{
    [SerializeField] private Unit unit;
    [SerializeField] private IntContainer currentActionPoints;
    public override List<Cell> Take()
    {
        return PathFinder.Instance.GetCellsAreaByActionPoints(unit.currentCell, currentActionPoints.Content);
    }
}
