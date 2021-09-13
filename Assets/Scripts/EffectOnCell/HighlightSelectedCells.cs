using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSelectedCells : HighlightValidateCells
{   
    public override void ApplyEffect()
    {
        TurnOffHighlight();
        cellsTaker.Take().ForEach(cell => cell.EnableSelectedCellHighlight(true));
    }
    
    public override void TurnOffHighlight()
    {
        Map.Instance.allCells.ForEach(cell => cell.EnableSelectedCellHighlight(false));
    }
}
