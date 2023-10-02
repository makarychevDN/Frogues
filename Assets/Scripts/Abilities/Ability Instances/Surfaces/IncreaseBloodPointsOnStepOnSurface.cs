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
        }

        private void TryToIncreaseBloodPoints(Unit unit)
        {
            if (unit.BloodPoints == null)
                return;

            if (unit.BloodPoints.Full)
                return;

            unit.BloodPoints.IncreasePoints(increaseValue);
            _owner.AbleToDie.DieWithoutAnimation();
        }
    }
}