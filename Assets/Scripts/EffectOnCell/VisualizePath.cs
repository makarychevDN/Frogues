using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizePath : BaseCellsEffect
{
    [SerializeField] private FindWayInValidCells pathFinderInValidCells;

    public override void ApplyEffect()
    {
        TurnOffVisualization();
        if (pathFinderInValidCells.Take() != null)
            pathFinderInValidCells.Take().ForEach(cell => cell.EnablePathDot(true));
    }
    
    public void TurnOffVisualization()
    {
        MapBasedOnTilemap.Instance.allCells.ForEach(cell => cell.EnablePathDot(false));
    }
}
