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
            cellTaker.Take().ForEach(cell => cell.EnableOnMouseHoverVisualization(true));
    }
    
    public void TurnOffVizualisation()
    {
        MapBasedOnTilemap.Instance.allCells.ForEach(cell => cell.EnableOnMouseHoverVisualization(false));
    }
}
