using UnityEngine;

namespace FroguesFramework
{
    public class MoveForHealthInsteadOfActionPointsAbility : NonTargetAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int healthCostDelta;
        [SerializeField] private int actionPointsCostDelta;
        private bool _isActive;

        public int GetValue() => healthCostDelta;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            unit.AbleToSkipTurn.OnSkipTurn.AddListener(UnUse);
            EntryPoint.Instance.OnNextRoomStarted.AddListener(UnUse);
        }

        public override void UnInit()
        {
            UnUse();
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(UnUse);
            EntryPoint.Instance.OnNextRoomStarted.RemoveListener(UnUse);
            base.UnInit();
        }

        public override void Use()
        {
            _owner.MovementAbility.IncreaseActionPointsCost(actionPointsCostDelta);
            _owner.MovementAbility.IncreaseHealthCost(healthCostDelta);
            _isActive = true;

            SpendResourcePoints();
            SetCooldownAsAfterUse();
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
        }

        private void UnUse()
        {
            if (!_isActive)
                return;

            _owner.MovementAbility.IncreaseActionPointsCost(-actionPointsCostDelta);
            _owner.MovementAbility.IncreaseHealthCost(-healthCostDelta);
            _isActive = false;
        }
    }
}