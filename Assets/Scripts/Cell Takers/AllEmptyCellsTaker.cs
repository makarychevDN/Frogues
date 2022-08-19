using System.Collections.Generic;
using System.Linq;

namespace FroguesFramework
{
    public class AllEmptyCellsTaker : BaseCellsTaker
    {
        public override List<Cell> Take()
        {
            var cells = Map.Instance.allCells;
            return cells.Where(cell => cell.CheckColumnIsEmpty()).Where(cells => cells.chosenToMovement == false).ToList();
        }
    }
}