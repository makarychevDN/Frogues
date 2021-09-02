using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : BaseCellsEffect
{
    [SerializeField] private BaseCellsTaker _cellTaker;
    
    public override void ApplyEffect()
    {
        TurnOffHighlight();
        _cellTaker.Take().ForEach(cell => cell.EnableHighlight(true));
    }
    
    public void TurnOffHighlight()
    {
        MapBasedOnTilemap.Instance._allCells.ForEach(cell => cell.EnableHighlight(false));
    }
}
