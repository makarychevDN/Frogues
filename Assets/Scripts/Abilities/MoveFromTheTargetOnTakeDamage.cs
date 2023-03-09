using UnityEngine;

namespace FroguesFramework
{
    public class MoveFromTheTargetOnTakeDamage : MonoBehaviour, IAbility
    {
        private Unit _target;
        private Unit _user;
        private Cell _mostFarFromTargetCell;

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
            
            _mostFarFromTargetCell = _user.CurrentCell;
            var neighborCells = CellsTaker.TakeCellsAreaByRange(_user.CurrentCell, 1).EmptyCellsOnly();

            foreach (var cell in neighborCells)
            {
                if (_target.CurrentCell.DistanceToCell(cell) >
                    _target.CurrentCell.DistanceToCell(_mostFarFromTargetCell))
                    _mostFarFromTargetCell = cell;
            }
            
            if(_mostFarFromTargetCell == _user.CurrentCell)
                return;
            
            Invoke(nameof(MoveAfterDelay), 0.25f);
        }

        private void MoveAfterDelay()
        {
            _user.Movable.Move(_mostFarFromTargetCell);
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