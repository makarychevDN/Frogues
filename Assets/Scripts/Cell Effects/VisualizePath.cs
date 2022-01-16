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
        if (cells == null)
            return;

        cells.GetLast().EnablePathDot(true);

        if (cells.Count == 1)
            return;

        for (int i = 1; i < cells.Count -1; i++)
        {
            cells[i].EnableTrail(cells[i - 1].coordinates - cells[i].coordinates);
            cells[i].EnableTrail(cells[i + 1].coordinates - cells[i].coordinates);
        }

        cells[0].EnableTrail(cells[1].coordinates - cells[0].coordinates);
        cells[cells.Count - 1].EnableTrail(cells[cells.Count - 2].coordinates - cells[cells.Count - 1].coordinates);
    }

    public void TurnOffVisualization()
    {
        Map.Instance.allCells.ForEach(cell =>
        {
            cell.DisableTrails();
            cell.EnablePathDot(false);
        });
    }
}
