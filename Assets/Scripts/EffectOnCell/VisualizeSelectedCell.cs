using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeSelectedCell : BaseCellsEffect
{
    [SerializeField] private CellByMousePosition _cellTaker;

    public override void ApplyEffect()
    {
        TurnOffVizualisation();
        if (_cellTaker.Take() != null)
            _cellTaker.Take().ForEach(cell => cell.EnableSelectedVisualization(true));
    }
    
    public void TurnOffVizualisation()
    {
        MapBasedOnTilemap.Instance._allCells.ForEach(cell => cell.EnableSelectedVisualization(false));
    }
}
