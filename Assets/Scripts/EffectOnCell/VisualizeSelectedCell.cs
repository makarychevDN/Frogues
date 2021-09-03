using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeSelectedCell : BaseCellsEffect
{
    [SerializeField] private CellByMousePosition cellTaker;

    public override void ApplyEffect()
    {
        TurnOffVizualisation();
        if (cellTaker.Take() != null)
            cellTaker.Take().ForEach(cell => cell.EnableSelectedVisualization(true));
    }
    
    public void TurnOffVizualisation()
    {
        MapBasedOnTilemap.Instance._allCells.ForEach(cell => cell.EnableSelectedVisualization(false));
    }
}
