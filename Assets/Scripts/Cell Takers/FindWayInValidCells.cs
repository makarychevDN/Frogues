using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FindWayInValidCells : BaseCellsTaker
    {
        [SerializeField] private BaseCellsTaker validCellsTaker;
        [SerializeField] private CellByMousePosition choosedCellTaker;
        [SerializeField] private Unit user;
        [SerializeField] private bool includeUserCell;
        [SerializeField] private bool ignoreDefaultUnits, ignoreProjectiles, ignoreSurfaces;

        private Cell _choosedCell;

        public override List<Cell> Take()
        {
            if (choosedCellTaker.Take() == null)
                return null;

            _choosedCell = choosedCellTaker.Take()[0];
            if (validCellsTaker.Take().Contains(_choosedCell))
            {
                return CalculateWay();
            }

            return null;
        }

        private List<Cell> CalculateWay()
        {
            var path = PathFinder.Instance.FindWay(user.currentCell, _choosedCell, ignoreDefaultUnits,
                ignoreProjectiles, ignoreSurfaces);

            if (includeUserCell)
                path.Insert(0, user.currentCell);

            return path;
        }
    }
}
