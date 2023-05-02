using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class MoveFromTargetOnTakeDamage : PassiveAbility
    {
        [SerializeField] private Unit _target;
        private Cell _mostFarFromTargetNeighborCells;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            if (_target == null)
                _target = EntryPoint.Instance.MetaPlayer;

            unit.Health.OnApplyUnblockedDamage.AddListener(() => TryToMoveFromTarget(_target));
        }

        private void TryToMoveFromTarget(Unit target)
        {
            if (_owner.Health.CurrentHp <= 0)
                return;

            var mostFarCells = new List<Cell>() { _owner.CurrentCell };
            var neighborCells = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, 1).EmptyCellsOnly();
            var farestDistance = target.CurrentCell.DistanceToCell(_owner.CurrentCell);

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

            if (mostFarCells.Contains(_owner.CurrentCell))
                return;

            _mostFarFromTargetNeighborCells = mostFarCells.GetRandomElement();
            Invoke(nameof(MoveAfterDelay), 0.25f);
        }

        private void MoveAfterDelay()
        {
            _owner.Movable.Move(_mostFarFromTargetNeighborCells);
        }
    }
}