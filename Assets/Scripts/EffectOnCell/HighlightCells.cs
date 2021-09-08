using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : BaseCellsEffect
{
    [SerializeField] protected BaseCellsTaker cellsTaker;
    
    public override void ApplyEffect()
    {
        TurnOffHighlight();
        cellsTaker.Take().ForEach(cell => cell.EnableDefaultHighlight(true));
    }
    
    public virtual void TurnOffHighlight()
    {
        MapBasedOnTilemap.Instance.allCells.ForEach(cell => cell.EnableDefaultHighlight(false));
    }
}
