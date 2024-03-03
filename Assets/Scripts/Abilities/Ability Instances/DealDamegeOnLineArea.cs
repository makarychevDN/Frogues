using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class DealDamegeOnLineArea : DefaultRoundAOEAbility
    {
        [SerializeField] private int range = int.MaxValue;

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, CellsTaker.ObstacleMode.noObstacles, false, true, range);
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            return CellsTaker.TakeCellsLineWhichContainCell(_owner.CurrentCell, cells[0], CellsTaker.ObstacleMode.noObstacles, false, true, range);
        }
    }
}