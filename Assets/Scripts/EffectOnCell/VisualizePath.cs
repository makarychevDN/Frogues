using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizePath : BaseCellsEffect
{
    [SerializeField] private FindWayInValidCells _pathFinderInValidCells;

    public override void ApplyEffect()
    {
        TurnOffVisualization();
        if (_pathFinderInValidCells.Take() != null)
            _pathFinderInValidCells.Take().ForEach(cell => cell.EnablePathDot(true));
    }
    
    public void TurnOffVisualization()
    {
        MapBasedOnTilemap.Instance._allCells.ForEach(cell => cell.EnablePathDot(false));
    }
}
