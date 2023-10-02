using UnityEngine;

namespace FroguesFramework
{
    public class BacklerPassiveAbility : PassiveAbility, IRoundTickable
    {
        [SerializeField] private int decreaseDeffenceToDamageSourceValue;
        [SerializeField] private int timeToEndEffect = 1;
        private bool _deffenceDecreasedOnThisTurnAlready;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            unit.Health.OnDamageFromUnitBlockedSuccessfully.AddListener(DecreaseDeffenceToDamageSource);
        }

        public void DecreaseDeffenceToDamageSource(Unit damageSource)
        {
            if (_deffenceDecreasedOnThisTurnAlready || damageSource == null)
                return;

            damageSource.Stats.AddStatEffect(StatEffectTypes.defence, -decreaseDeffenceToDamageSourceValue, timeToEndEffect);
            _deffenceDecreasedOnThisTurnAlready = true;
        }

        public void TickAfterEnemiesTurn()
        {
            if (!_owner.IsEnemy)
                _deffenceDecreasedOnThisTurnAlready = false;
        }

        public void TickAfterPlayerTurn()
        {
            if (_owner.IsEnemy)
                _deffenceDecreasedOnThisTurnAlready = false;
        }
    }
}