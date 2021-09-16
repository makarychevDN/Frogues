using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizePath : BaseCellsEffect
{
    [SerializeField] private FindWayInValidCells pathFinderInValidCells;

    public override void ApplyEffect()
    {
        ApplyEffect(pathFinderInValidCells.Take());
    }

    public override void ApplyEffect(List<Cell> cells)
    {
        TurnOffVisualization();
        if (cells != null)
            cells.ForEach(cell => cell.EnablePathDot(true));
    }

    public void TurnOffVisualization()
    {
        Map.Instance.allCells.ForEach(cell => cell.EnablePathDot(false));
    }
}
