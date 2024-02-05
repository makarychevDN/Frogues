using System.Collections.Generic;
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

            var mostFarCells = new List<Cell>() { _unit.CurrentCell };
            var neighborCells = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, 1).EmptyCellsOnly();
            var farestDistance = target.CurrentCell.DistanceToCell(_unit.CurrentCell);

            foreach (var cell in neighborCells)
            {
                if (target.CurrentCell.DistanceToCell(cell) > farestDistance)
                {
                    mostFarCells.Clear();
                    farestDistance = target.CurrentCell.DistanceToCell(cell);
                }

                if (target.CurrentCell.DistanceToCell(cell) == farestDistance)
                    mostFarCells.Add(cell);
            }

            if (mostFarCells.Contains(_unit.CurrentCell))
            {
                EndTurn();
                return;
            }

            _unit.MovementAbility.CalculateUsingArea();
            _unit.MovementAbility.UseOnCells(new List<Cell> { mostFarCells.GetRandomElement() });
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