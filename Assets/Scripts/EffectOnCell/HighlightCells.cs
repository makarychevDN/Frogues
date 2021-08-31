using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : BaseCellsEffect
{
    [SerializeField] private BaseCellsTaker _cellTaker;

    public override void ApplyEffect()
    {
        MapBasedOnTilemap.Instance._layers.ForEach(layer =>
        {
            foreach (var cell in layer)
            {
                cell.EnableHighlight(false);
            }
        });

        _cellTaker.Take().ForEach(cell => { cell.EnableHighlight(true); /*cell.EnablePathDot(true);*/ });
    }
}
