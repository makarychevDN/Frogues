using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitCellTaker : BaseCellsTaker
    {
        [SerializeField] private Unit unit;

        public override List<Cell> Take() => new List<Cell>() {unit.currentCell};
    }
}