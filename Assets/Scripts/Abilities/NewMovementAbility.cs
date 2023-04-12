using System.Collections.Generic;

namespace FroguesFramework
{
    public class NewMovementAbility : AreaTargetAbility
    {
        private List<Cell> _currentPath = new List<Cell>();

        public override List<Cell> CalculateUsingArea()
        {
            _usingArea = EntryPoint.Instance.PathFinder.GetCellsAreaByActionPoints(_owner.CurrentCell,
            _owner.ActionPoints.CurrentActionPoints,
                _owner.Movable.DefaultMovementCost, false, true, true);
            return _usingArea;
        }

        public override void DisablePreVisualization(){}

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            if (cells == null || cells.Count == 0)
                return false;

            if (!_usingArea.Contains(cells.GetLast()))
                return false;

            return true;
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            if (cells == null || cells.Count == 0)
                return null;

            return EntryPoint.Instance.PathFinder.FindWay(_owner.CurrentCell, cells.GetLast(), false, false, false);
        }

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            _currentPath = cells;
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _usingArea.ForEach(cell => cell.EnableValidForMovementCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            cells.Insert(0, _owner.CurrentCell);

            for (int i = 1; i < cells.Count - 1; i++)
            {
                cells[i].EnableTrail(cells[i - 1]);
                cells[i].EnableTrail(cells[i + 1]);
            }

            cells.GetLast().EnableTrail(cells[cells.Count - 2]);
            cells[0].EnableTrail(cells[1]);
            cells.GetLast().EnablePathDot(true);

            _owner.ActionPoints.PreSpendPoints((cells.Count - 1) * 2);
        }

        public bool IsMoving => _currentPath.Count != 0;

        private void Update()
        {
            if (!CurrentlyActiveObjects.SomethingIsActNow && IsMoving)
            {
                _owner.Movable.Move(_currentPath[0]);
                _currentPath.RemoveAt(0);
            }
        }
    }
}