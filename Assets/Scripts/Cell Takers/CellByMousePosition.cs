using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CellByMousePosition : BaseCellsTaker
    {
        [SerializeField] private Grid grid;
        [SerializeField] private bool takeColumn;

        public override List<Cell> Take()
        {
            if (grid == null) grid = Map.Instance.tilemap.layoutGrid;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);

            try
            {
                return takeColumn
                    ? Map.Instance.GetCellsColumn(coordinate.ToVector2Int())
                    : new List<Cell> {Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y]};
            }
            catch
            {
                return null;
            }
        }
    }
}
