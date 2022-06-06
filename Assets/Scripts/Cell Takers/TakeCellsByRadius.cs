using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TakeCellsByRadius : BaseCellsTaker
    {
        [SerializeField] private BaseCellsTaker startCellsTaker;
        [SerializeField] private bool includeStartCell;
        [SerializeField, Range(1, 10)] private int radius;
        [SerializeField] private bool ignoreCellIsBusy;
        [Header("For Isometric Maps Only")]
        [SerializeField] private bool diagonalStep;

        public override List<Cell> Take()
        {
            if (startCellsTaker.Take() == null)
                return new List<Cell>();
            
            var cellsList = PathFinder.Instance.GetCellsAreaForAOE(startCellsTaker.Take().FirstOrDefault(), radius, ignoreCellIsBusy, diagonalStep);
            
            if(includeStartCell)
                cellsList.Add(startCellsTaker.Take().FirstOrDefault());

            return cellsList;
        }
    }
}