using System.Collections.Generic;

namespace FroguesFramework
{
    public class WaterStringAbility : DefaultRoundAOEAbility
    {
        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, CellsTaker.ObstacleMode.noObstacles, false, true);
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            return CellsTaker.TakeCellsLineWhichContainCell(_owner.CurrentCell, cells[0], CellsTaker.ObstacleMode.noObstacles, false, true);
        }
    }
}