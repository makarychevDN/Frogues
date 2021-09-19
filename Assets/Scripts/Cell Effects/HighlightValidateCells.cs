using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightValidateCells : BaseCellsEffect
{
    [SerializeField] protected BaseCellsTaker cellsTaker;
    
    public override void ApplyEffect()
    {
        ApplyEffect(cellsTaker.Take());
    }

    public override void ApplyEffect(List<Cell> cells)
    {
        TurnOffHighlight();
        cellsTaker.Take().ForEach(cell => cell.EnableValidateCellHighlight(true));
    }

    public virtual void TurnOffHighlight()
    {
        Map.Instance.allCells.ForEach(cell => cell.EnableValidateCellHighlight(false));
    }
}
