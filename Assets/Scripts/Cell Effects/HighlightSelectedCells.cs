using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class HighlightSelectedCells : BaseCellsEffect
    {
        [SerializeField] private BaseCellsTaker cellsTaker;

        public override void ApplyEffect() => ApplyEffect(cellsTaker.Take());

        public override void ApplyEffect(List<Cell> cells)
        {
            TurnOffHighlight();
            cells.ForEach(cell => cell.EnableSelectedCellHighlight(true));
        }

        public void TurnOffHighlight()
        {
            Map.Instance.allCells.ForEach(cell => cell.EnableSelectedCellHighlight(false));
        }
    }
}