using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCellsByRadius : BaseCellsTaker
{
    [SerializeField] private Unit unit;
    [SerializeField, Range(1, 10)] private int radius;
    [SerializeField] private bool ignoreCellIsBusy;
    [SerializeField] private bool diagonalStep;

    private List<Vector2Int> _dirVectors = new List<Vector2Int>()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    
    public override List<Cell> Take()
    {
        return PathFinder.Instance.GetCellsAreaForAOE(unit.currentCell, radius, ignoreCellIsBusy, diagonalStep);
    }
}
