using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightValidateCells : BaseCellsEffect
{
    [SerializeField] protected BaseCellsTaker cellsTaker;
    
    public override void ApplyEffect()
    {
        TurnOffHighlight();
        cellsTaker.Take().ForEach(cell => cell.EnableValidateCellHighlight(true));
    }
    
    public virtual void TurnOffHighlight()
    {
        MapBasedOnTilemap.Instance.allCells.ForEach(cell => cell.EnableValidateCellHighlight(false));
    }
}
