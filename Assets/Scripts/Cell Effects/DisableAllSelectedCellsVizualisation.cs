using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllSelectedCellsVizualisation : BaseCellsEffect
{
    public override void ApplyEffect()
    {
        ApplyEffect(Map.Instance.allCells);
    }

    public override void ApplyEffect(List<Cell> cells)
    {
        cells.ForEach(cell => cell.DisableAllVisualization());
    }
}
