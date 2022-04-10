using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class HighlightValidForMovementCells : BaseCellsEffect
    {
        [SerializeField] protected BaseCellsTaker cellsTaker;

        public override void ApplyEffect()
        {
            ApplyEffect(cellsTaker.Take());
        }

        public override void ApplyEffect(List<Cell> cells)
        {
            TurnOffHighlight();
            cellsTaker.Take().ForEach(cell => cell.EnableValidForMovementCellHighlight(true));
        }

        public virtual void TurnOffHighlight()
        {
            Map.Instance.allCells.ForEach(cell => cell.EnableValidForMovementCellHighlight(false));
        }
    }
}