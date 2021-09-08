using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHighlightCells : HighlightCells
{
    [SerializeField] private Color highlightColor;
    
    public override void ApplyEffect()
    {
        TurnOffHighlight();
        cellsTaker.Take().ForEach(cell => cell.EnableCustomHighlight(true, highlightColor));
    }
    
    public override void TurnOffHighlight()
    {
        MapBasedOnTilemap.Instance.allCells.ForEach(cell => cell.EnableCustomHighlight(false));
    }
}
