using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : BaseCellsEffect
{
    [SerializeField] private BaseCellsTaker _cellTaker;

    public override void ApplyEffect()
    {
        MapBasedOnTilemap.Instance._allCells.ForEach(cell => cell.EnableHighlight(false));
        _cellTaker.Take().ForEach(cell => cell.EnableHighlight(true));
    }
}
