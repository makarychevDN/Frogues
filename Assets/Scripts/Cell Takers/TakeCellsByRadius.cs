using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TakeCellsByRadius : BaseCellsTaker
    {
        [SerializeField] private Unit unit;
        [SerializeField, Range(1, 10)] private int radius;
        [SerializeField] private bool ignoreCellIsBusy;
        [Header("For Isometric Maps Only")]
        [SerializeField] private bool diagonalStep;

        public override List<Cell> Take()
        {
            return PathFinder.Instance.GetCellsAreaForAOE(unit.currentCell, radius, ignoreCellIsBusy, diagonalStep);
        }
    }
}