using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitContainerCellTaker : BaseCellsTaker
    {
        [SerializeField] private UnitContainer unitContainer;

        public override List<Cell> Take() => new List<Cell>() {unitContainer.Content.currentCell};
    }
}