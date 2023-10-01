using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikedBallAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private AnimationCurve animationCurve;

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, true);
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (target != null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(CalculateDamage, ignoreArmor);
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            //lineFromOwnerToTarget.SetAnimationCurveShape(_owner.transform.position, _owner.transform.position, target.transform.position, 1, animationCurve);
            
            var dir = CellsTaker.GetHexDirByOtherCell(_owner.CurrentCell, target.CurrentCell, true);
            lineFromOwnerToTarget.SetAnimationCurveShape(_owner.transform.position, _owner.transform.position, target.transform.position, 1, animationCurve);
        }

        public override bool PossibleToUseOnUnit(Unit target)
        {
            bool value = base.PossibleToUseOnUnit(target);

            if (value == false)
                return false;

            if (target.CurrentCell.DistanceToCell(_owner.CurrentCell) == 1)
                return false;

            return true;
        }
    }
}