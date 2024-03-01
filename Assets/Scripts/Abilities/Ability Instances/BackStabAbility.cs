using System.Collections.Generic;

namespace FroguesFramework
{
    public class BackStabAbility : JumpOnCellAbility, IAbleToHashUnitTarget
    {
        private Unit _hashedTarget;
        private int _damage;

        public void HashUnitTargetAndCosts(Unit target, int actionPointsCost, int bloodPointsCost, int damage)
        {
            _hashedTarget = target;
            this.actionPointsCost = actionPointsCost;
            this.bloodPointsCost = bloodPointsCost;
            _damage = damage;
        }

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = EntryPoint.Instance.PathFinder.GetCellsAreaByRange(_hashedTarget.CurrentCell, range, false, false, true);
        }

        public override bool IsResoursePointsEnough()
        {
            if (_owner.BloodPoints != null)
            {
                return _owner.ActionPoints.IsPointsEnough(actionPointsCost)
                    && _owner.BloodPoints.IsPointsEnough(bloodPointsCost);
            }

            return _owner.ActionPoints.IsPointsEnough(actionPointsCost);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.ActionPoints.PreSpendPoints(CalculateActionPointsCost + _owner.AbilitiesManager.WeaponActionPointsCost);
            _owner.BloodPoints.PreSpendPoints(CalculateBloodPointsCost);

            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetAnimationCurveShape(_owner.SpriteParent.position, cells[0].transform.position,
                movementHeight * _owner.CurrentCell.DistanceToCell(cells[0]), parabolaAnimationCurve);
            cells[0].EnableSelectedByAbilityCellHighlight(new List<Cell> { cells[0] });

            _hashedTarget.Health.PreTakeDamage(_damage);
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

            if (ableToHaveNativeAtack.GetCurrentNativeAttack() != null)
            {
                ableToHaveNativeAtack.GetCurrentNativeAttack().PrepareToUsing(_hashedTarget);
                ableToHaveNativeAtack.GetCurrentNativeAttack().UseOnUnit(_hashedTarget);
            }

            _owner.Movable.OnMovementEnd.RemoveListener(UseNativeAttack);
        }

        public override bool IsIgnoringDrawingFunctionality() => true;
    }
}