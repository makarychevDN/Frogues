using UnityEngine;

namespace FroguesFramework
{
    public class BucklerPassiveAbility : PassiveAbility, IRoundTickable, IAbleToApplyDefenceModificator
    {
        [SerializeField] private int decreaseDeffenceToDamageSourceValue;
        [SerializeField] private int timeToEndEffect = 1;
        private bool _deffenceDecreasedOnThisTurnAlready;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            unit.Health.OnDamageFromUnitPreventedByBlock.AddListener(DecreaseDeffenceToDamageSource);
        }

        public override void UnInit()
        {
            _owner.Health.OnDamageFromUnitPreventedByBlock.RemoveListener(DecreaseDeffenceToDamageSource);
            base.UnInit();
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

        public int GetDefenceModificatorValue() => decreaseDeffenceToDamageSourceValue;

        public int GetdeltaOfDefenceValueForEachTurn() => 0;

        public int GetTimeToEndOfDefenceEffect() => timeToEndEffect;

        public bool GetDefenceEffectIsConstantly() => false;
    }
}