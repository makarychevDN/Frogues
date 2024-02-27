namespace FroguesFramework
{
    public class ShieldNativeAttack : DefaultCritableUnitTargetAbility
    {
        public override void Init(Unit unit)
        {
            base.Init(unit);
            OnEffectApplied.AddListener(TurnOffCriticalMode);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TurnOffCriticalMode);
        }

        public override void UnInit()
        {
            OnEffectApplied.RemoveListener(TurnOffCriticalMode);
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TurnOffCriticalMode);
            base.UnInit();
        }

        public override void TickAfterEnemiesTurn()
        {
            base.TickAfterEnemiesTurn();

            if (_owner == null)
                return;

            if (!_owner.IsEnemy)
            {
                WaitAttackOwnerModeOff();
            }
            else
            {
                WaitAttackOwnerModeOn();
            }
        }

        public override void TickAfterPlayerTurn()
        {
            base.TickAfterPlayerTurn();

            if (_owner == null)
                return;

            if (_owner.IsEnemy)
            {
                WaitAttackOwnerModeOff();
            }
            else
            {
                WaitAttackOwnerModeOn();
            }
        }

        private void WaitAttackOwnerModeOn()
        {
            _owner.Health.OnDamagePreventedByBlock.AddListener(TurnOnCriticalMode);
            _owner.Health.OnDamageAppliedByArmor.AddListener(TurnOffCriticalMode);
            _owner.Health.OnDamageAppledByHealth.AddListener(TurnOffCriticalMode);
        }

        private void WaitAttackOwnerModeOff()
        {
            _owner.Health.OnDamagePreventedByBlock.RemoveListener(TurnOnCriticalMode);
            _owner.Health.OnDamageAppliedByArmor.RemoveListener(TurnOffCriticalMode);
            _owner.Health.OnDamageAppledByHealth.RemoveListener(TurnOffCriticalMode);
        }
    }
}