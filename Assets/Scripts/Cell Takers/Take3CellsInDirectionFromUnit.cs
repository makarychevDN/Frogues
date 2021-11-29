using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take3CellsInDirectionFromUnit : CellTakerByDirection
{
    [SerializeField] private Unit unit;
    [SerializeField] private SpriteRotator spriteRotator;

    public override List<Cell> Take()
    {
        return Take(currentDirectionContainer.Content);
    }

    public override List<Cell> Take(Vector2Int direction)
    {
        List<Cell> result = new List<Cell>();

        var startCell = Map.Instance.FindNeigborhoodForCell(unit.currentCell, direction);
        result.Add(startCell);
        
        if(direction.x != 0)
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(startCell, new Vector2Int(0, 1)));
            result.Add(Map.Instance.FindNeigborhoodForCell(startCell, new Vector2Int(0, -1)));
        }
        else
        {
            result.Add(Map.Instance.FindNeigborhoodForCell(startCell, new Vector2Int(1, 0)));
            result.Add(Map.Instance.FindNeigborhoodForCell(startCell, new Vector2Int(-1, 0)));
        }

        if(direction == Vector2Int.left || direction == Vector2Int.up)
        {
            spriteRotator.TurnLeft();
        }
        else
        {
            spriteRotator.TurnRight();
        }



        return result;
    }
}
