using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class Take3CellsInDirectionFromUnit : CellTakerByDirection
    {
        [SerializeField] private Unit unit;

        public override List<Cell> Take()
        {
            return Take(currentDirectionContainer.Content);
        }

        public override List<Cell> Take(Vector2Int direction)
        {
            List<Cell> result = new List<Cell>();

            var startCell = Map.Instance.FindNeighborhoodForCell(unit.currentCell, direction);
            result.Add(startCell);

            if (direction.x != 0)
            {
                result.Add(Map.Instance.FindNeighborhoodForCell(startCell, new Vector2Int(0, 1)));
                result.Add(Map.Instance.FindNeighborhoodForCell(startCell, new Vector2Int(0, -1)));
            }
            else
            {
                result.Add(Map.Instance.FindNeighborhoodForCell(startCell, new Vector2Int(1, 0)));
                result.Add(Map.Instance.FindNeighborhoodForCell(startCell, new Vector2Int(-1, 0)));
            }

            return result;
        }
    }
}
