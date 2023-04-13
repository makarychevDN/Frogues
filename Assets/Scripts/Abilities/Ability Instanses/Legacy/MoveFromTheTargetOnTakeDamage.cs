using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class MoveFromTheTargetOnTakeDamage : MonoBehaviour
    {
        private Unit _target;
        private Unit _user;
        private Cell _mostFarFromTargetNeighborCells;

        public void VisualizePreUse()
        {
            throw new System.NotImplementedException();
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        public void Init(Unit unit)
        {
            _target = EntryPoint.Instance.MetaPlayer;
            _user = unit;
            unit.Health.OnApplyUnblockedDamage.AddListener(TryToMoveFromTarget);
        }

        private void TryToMoveFromTarget()
        {
            if(_user.Health.CurrentHp <= 0)
                return;

            var mostFarCells = new List<Cell>() { _user.CurrentCell };
            var neighborCells = CellsTaker.TakeCellsAreaByRange(_user.CurrentCell, 1).EmptyCellsOnly();
            var farestDistance = _target.CurrentCell.DistanceToCell(_user.CurrentCell);

            foreach (var cell in neighborCells)
            {
                if (_target.CurrentCell.DistanceToCell(cell) > farestDistance)
                {
                    mostFarCells.Clear();
                    farestDistance = _target.CurrentCell.DistanceToCell(cell);
                }

                if (_target.CurrentCell.DistanceToCell(cell) == farestDistance)
                    mostFarCells.Add(cell);
            }
            
            if(mostFarCells.Contains(_user.CurrentCell))
                return;

            _mostFarFromTargetNeighborCells = mostFarCells.GetRandomElement();            
            Invoke(nameof(MoveAfterDelay), 0.25f);
        }

        private void MoveAfterDelay()
        {
            _user.Movable.Move(_mostFarFromTargetNeighborCells);
        }

        public int GetCost()
        {
            throw new System.NotImplementedException();
        }

        public bool IsPartOfWeapon()
        {
            throw new System.NotImplementedException();
        }
    }
}