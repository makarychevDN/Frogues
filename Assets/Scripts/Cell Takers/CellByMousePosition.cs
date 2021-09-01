using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellByMousePosition : BaseCellsTaker
{
    public Grid grid;

    public override List<Cell> Take()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);

        try
        {
            var cells = new List<Cell> {MapBasedOnTilemap.Instance._unitsLayer[coordinate.x, coordinate.y]};
            return cells;
        }
        catch
        {
            return null;
        }
    }
}
