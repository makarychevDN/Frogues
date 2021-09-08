using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCellsByRadius : BaseCellsTaker
{
    [SerializeField] private Unit unit;
    [SerializeField] private int radius;

    private List<Vector2Int> _dirVectors = new List<Vector2Int>()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    
    public override List<Cell> Take()
    {
        List<Cell> cells = new List<Cell>();
        //int count = 0;

        /*while (count != radius)
        {
            foreach (var cell in cells)
            {
                foreach (var dirVector in _dirVectors)
                {
                    if (!cells.Contains(MapBasedOnTilemap.Instance.FindNeigborhoodForCell(cell, dirVector)))
                    {
                        cells.Add(MapBasedOnTilemap.Instance.FindNeigborhoodForCell(cell, dirVector));
                    }
                }
            }
            count++;
        }*/
        
        foreach (var dirVector in _dirVectors)
        {
            cells.Add(MapBasedOnTilemap.Instance.FindNeigborhoodForCell(unit.currentCell, dirVector));
        }
        
        return cells;
    }
}
