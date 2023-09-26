using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.MaterialProperty;

namespace FroguesFramework
{
    public class WaterStringAbility : DefaultRoundAOEAbility
    {
        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, true);
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            return CellsTaker.TakeCellsLineWhichContainCell(_owner.CurrentCell, cells[0], true);
        }
    }
}