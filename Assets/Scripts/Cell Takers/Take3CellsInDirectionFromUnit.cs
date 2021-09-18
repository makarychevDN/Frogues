using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take3CellsInDirectionFromUnit : BaseCellsTaker
{
    [SerializeField] private Unit unit;

    public override List<Cell> Take()
    {
        Vector3 unitPos = unit.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Cell> result = new List<Cell>();

        if (mousePos.x < unitPos.x && mousePos.y < unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, 0)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, -1)));
        }

        if (mousePos.x > unitPos.x && mousePos.y < unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, -1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(0, -1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, -1)));
        }

        if (mousePos.x < unitPos.x && mousePos.y > unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(0, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, 1)));
        }

        if (mousePos.x > unitPos.x && mousePos.y > unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, 0)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, -1)));
        }

        return result;
    }
}
