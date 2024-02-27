using UnityEngine;

namespace FroguesFramework
{
    public class PickUpTemporaryActionPointsOnStepOnSurface : PassiveAbility
    {
        //[SerializeField] private int increaseValue;
        [SerializeField] private int additionalTemporaryActionPointsQuantity;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            unit.OnStepOnThisUnitByUnit.AddListener(TryToIncreaseTemporaryActionPoints);
            EntryPoint.Instance.AddBloodSurface(_owner);
        }

        private void TryToIncreaseTemporaryActionPoints(Unit unit)
        {
            /*if (unit.BloodPoints == null)
                return;

            if (unit.BloodPoints.Full)
                return;*/

            //unit.BloodPoints.PickupPoints(increaseValue);
            if (unit != EntryPoint.Instance.MetaPlayer)
                return;

            unit.ActionPoints.PickupTemporaryPoints(additionalTemporaryActionPointsQuantity);
            _owner.AbleToDie.DieWithoutAnimation();
            EntryPoint.Instance.RemoveBloodSurface(_owner);
        }
    }
}