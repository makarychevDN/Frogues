using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseBloodPointsOnStepOnSurface : PassiveAbility
    {
        [SerializeField] private int increaseValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            unit.OnStepOnThisUnitByUnit.AddListener(TryToIncreaseBloodPoints);
            EntryPoint.Instance.AddBloodSurface(_owner);
        }

        private void TryToIncreaseBloodPoints(Unit unit)
        {
            if (unit.BloodPoints == null)
                return;

            if (unit.BloodPoints.Full)
                return;

            unit.BloodPoints.PickupPoints(increaseValue);
            _owner.AbleToDie.DieWithoutAnimation();
            EntryPoint.Instance.RemoveBloodSurface(_owner);
        }
    }
}