using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take3CellsInDirectionFromUnit : BaseCellsTaker
{
    [SerializeField] private Unit unit;
    [SerializeField] private SpriteRotator spriteRotator;

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
            spriteRotator.TurnLeft();
        }

        if (mousePos.x > unitPos.x && mousePos.y < unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, -1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(0, -1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, -1)));
            spriteRotator.TurnRight();
        }

        if (mousePos.x < unitPos.x && mousePos.y > unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(0, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(-1, 1)));
            spriteRotator.TurnLeft();
        }

        if (mousePos.x > unitPos.x && mousePos.y > unitPos.y)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, 0)));
            result.Add(Map.Instance.FindNeigborhoodForCell(unit.currentCell, new Vector2Int(1, -1)));
            spriteRotator.TurnRight();
        }

        return result;
    }
}
