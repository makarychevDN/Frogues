using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AlternatesRunFromTargetAndDoSomethingAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] protected Unit target;
        protected Unit _unit;
        protected bool _moveFromTargetMode;

        public void Act()
        {
            if (_moveFromTargetMode)
            {
                TryToRunFromTarget();
            }
            else
            {
                TryToDoSomething();
            }
        }

        protected abstract void TryToDoSomething();

        protected virtual void TryToRunFromTarget()
        {
            if (_unit.MovementAbility == null || !_unit.MovementAbility.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            var theBestToRunCells = new List<Cell>() { _unit.CurrentCell };
            var neighborCells = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, 1).EmptyCellsOnly();
            var farestDistance = target.CurrentCell.DistanceToCell(_unit.CurrentCell);

            foreach (var cell in neighborCells)
            {
                if (target.CurrentCell.DistanceToCell(cell) > farestDistance)
                {
                    theBestToRunCells.Clear();
                    farestDistance = target.CurrentCell.DistanceToCell(cell);
                }

                if (target.CurrentCell.DistanceToCell(cell) == farestDistance)
                    theBestToRunCells.Add(cell);
            }

            if (theBestToRunCells.Contains(_unit.CurrentCell))
            {
                int leastBarriersQuantity = 6;

                foreach(var cell in neighborCells)
                {
                    int barriersQuantity = cell.CellNeighbours.GetAllNeighbors().Where(cell => cell.Content is Barrier).Count();

                    if (barriersQuantity < leastBarriersQuantity)
                    {
                        leastBarriersQuantity = barriersQuantity;
                        theBestToRunCells.Clear();
                        farestDistance = target.CurrentCell.DistanceToCell(cell);
                    }

                    if (target.CurrentCell.DistanceToCell(cell) == farestDistance)
                        theBestToRunCells.Add(cell);
                }

                if (theBestToRunCells.Contains(_unit.CurrentCell))
                {
                    EndTurn();
                    return;
                }
            }

            _unit.MovementAbility.CalculateUsingArea();
            _unit.MovementAbility.UseOnCells(new List<Cell> { theBestToRunCells.GetRandomElement() });
        }

        protected void EndTurn()
        {
            _unit.AbleToSkipTurn.AutoSkip();
            _moveFromTargetMode = !_moveFromTargetMode;
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}