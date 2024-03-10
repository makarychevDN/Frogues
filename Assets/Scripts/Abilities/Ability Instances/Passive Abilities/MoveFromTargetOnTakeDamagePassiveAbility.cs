using System.Collections.Generic;

namespace FroguesFramework
{
    public class MoveFromTargetOnTakeDamagePassiveAbility : PassiveAbility, IRoundTickable
    {
        private Cell _mostFarFromTargetNeighborCells;
        private bool _movedOnTakeDamageAlready;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            unit.Health.OnDamageFromUnitAppliedByHealth.AddListener(TryToMoveFromTarget);
        }

        public override void UnInit()
        {
            _owner.Health.OnDamageFromUnitAppliedByHealth.RemoveListener(TryToMoveFromTarget);
            base.UnInit();
        }

        private void TryToMoveFromTarget(Unit target)
        {
            if (target == null)
                return;

            if(_movedOnTakeDamageAlready) 
                return;

            if (_owner.Health.CurrentHp <= 0)
                return;

            var theBestCellsToRetreat = CellsTaker.GetBestCellsToRetreatFromTarget(_owner, target);           

            if (theBestCellsToRetreat.Contains(_owner.CurrentCell))
                return;

            EntryPoint.Instance.MetaPlayer.MovementAbility.ResetPath();
            _mostFarFromTargetNeighborCells = theBestCellsToRetreat.GetRandomElement();
            _movedOnTakeDamageAlready = true;
            Invoke(nameof(MoveAfterDelay), 0.24f);
        }

        private void MoveAfterDelay()
        {
            _owner.MovementAbility.CalculateUsingArea();
            _owner.MovementAbility.UseOnCells(new List<Cell> { _mostFarFromTargetNeighborCells });
        }

        public void TickAfterEnemiesTurn()
        {
            if (_owner.IsEnemy)
                _movedOnTakeDamageAlready = false;
            else
                _movedOnTakeDamageAlready = true;
        }

        public void TickAfterPlayerTurn()
        {
            if (!_owner.IsEnemy)
                _movedOnTakeDamageAlready = false;
            else
                _movedOnTakeDamageAlready = true;
        }
    }
}