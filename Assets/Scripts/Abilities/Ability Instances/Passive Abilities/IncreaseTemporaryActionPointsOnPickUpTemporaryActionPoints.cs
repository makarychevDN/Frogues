using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseTemporaryActionPointsOnPickUpTemporaryActionPoints : PassiveAbility
    {
        [SerializeField] private int additionalTemporaryActionPoints;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.ActionPoints.OnPickUpPoints.AddListener(AddTemporaryActionPoints);
        }

        public override void UnInit()
        {
            _owner.ActionPoints.OnPickUpPoints.RemoveListener(AddTemporaryActionPoints);
            base.UnInit();
        }

        private void AddTemporaryActionPoints()
        {
            _owner.ActionPoints.IncreaseTemporaryPoints(additionalTemporaryActionPoints);
        }
    }
}