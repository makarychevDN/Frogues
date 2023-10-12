using System.Collections.Generic;

namespace FroguesFramework
{
    public class BackStabAbility : JumpOnCellAbility, IAbleToHashUnitTarget
    {
        private Unit _hashedTarget;

        public void HashUnitTarget(Unit target) => _hashedTarget = target;

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = EntryPoint.Instance.PathFinder.GetCellsAreaByRange(_hashedTarget.CurrentCell, range, false, false, true);
        }

        protected override int CalculateActionPointsCost => _owner.AbilitiesManager.WeaponActionPointsCost == 2 ? 1 : 0;

        public override bool IsResoursePointsEnough()
        {
            if (_owner.BloodPoints != null)
            {
                return _owner.ActionPoints.IsPointsEnough(CalculateActionPointsCost + _owner.AbilitiesManager.WeaponActionPointsCost)
                    && _owner.BloodPoints.IsPointsEnough(CalculateBloodPointsCost);
            }

            return _owner.ActionPoints.IsPointsEnough(actionPointsCost + _owner.AbilitiesManager.WeaponActionPointsCost);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            base.VisualizePreUseOnCells(cells);
            _owner.ActionPoints.PreSpendPoints(CalculateActionPointsCost + _owner.AbilitiesManager.WeaponActionPointsCost);
        }

        public override void UseOnCells(List<Cell> cells)
        {
            _owner.Movable.OnMovementEnd.AddListener(UseNativeAttack);
            base.UseOnCells(cells);
            _owner.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
        }

        private void UseNativeAttack()
        {
            var ableToHaveNativeAtack = _owner.ActionsInput as IAbleToHaveNativeAttack;
            if (ableToHaveNativeAtack == null)
                return;

            ableToHaveNativeAtack.GetCurrentNativeAttack().PrepareToUsing(_hashedTarget);
            ableToHaveNativeAtack.GetCurrentNativeAttack().UseOnUnit(_hashedTarget);
            _owner.Movable.OnMovementEnd.RemoveListener(UseNativeAttack);
        }

        public override bool IsIgnoringDrawingFunctionality() => true;
    }
}