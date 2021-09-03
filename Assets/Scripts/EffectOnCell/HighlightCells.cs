using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : BaseCellsEffect
{
    [SerializeField] private BaseCellsTaker cellsTaker;
    
    public override void ApplyEffect()
    {
        TurnOffHighlight();
        cellsTaker.Take().ForEach(cell => cell.EnableHighlight(true));
    }
    
    public void TurnOffHighlight()
    {
        MapBasedOnTilemap.Instance.allCells.ForEach(cell => cell.EnableHighlight(false));
    }
}
