using System.Collections.Generic;

namespace FroguesFramework
{
    public class DisableAllSelectedCellsVizualisation : BaseCellsEffect
    {
        public override void ApplyEffect()
        {
            ApplyEffect(Map.Instance.allCells);
        }

        public override void ApplyEffect(List<Cell> cells)
        {
            cells.ForEach(cell => cell.DisableAllCellVisualization());
        }
    }
}
