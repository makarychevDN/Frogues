using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class DefaultAreaAbility : AreaTargetAbility
    {
        [SerializeField] private int damage;
        [SerializeField] private int radius;
        private List<Cell> _selectedArea;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            throw new System.NotImplementedException();
        }

        public override void UseOnCells(List<Cell> cells)
        {
            throw new System.NotImplementedException();
        }

        //public override void VisualizePreUse() => _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

        public override void DisablePreVisualization() { }

        public override List<Cell> SelectCells()
        {
            throw new System.NotImplementedException();
        }
    }
}
