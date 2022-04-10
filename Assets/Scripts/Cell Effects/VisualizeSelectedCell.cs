using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class VisualizeSelectedCell : BaseCellsEffect
    {
        [SerializeField] private CellByMousePosition cellTaker;

        public override void ApplyEffect()
        {
            ApplyEffect(cellTaker.Take());
        }

        public override void ApplyEffect(List<Cell> cells)
        {
            TurnOffVizualisation();
            if (cells != null)
                cells.ForEach(cell => cell.EnableOnMouseHoverVisualization(true));
        }

        public void TurnOffVizualisation()
        {
            Map.Instance.allCells.ForEach(cell => cell.EnableOnMouseHoverVisualization(false));
        }
    }
}