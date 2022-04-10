using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TakeCellInOtherLayerByUnit : BaseCellsTaker
    {
        [SerializeField] private Unit unit;
        [SerializeField] private MapLayer expectedLayer;

        public override List<Cell> Take()
        {
            return new List<Cell> {Map.Instance.GetCell(unit.Coordinates, expectedLayer)};
        }
    }
}