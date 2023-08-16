using System;
using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class KnifeNativeAttackAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private int critDamage;
        private bool _nextAttackIsCritical;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.AddListener(TurnOnCriticalMode);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.RemoveListener(TurnOnCriticalMode);
        }

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            target.Health.TakeDamage(CalculateDamage, _nextAttackIsCritical);
            TurnOffCriticalMode();
        }

        protected override int CalculateDamage => damageType == DamageType.physics
            ? (int)(GetDamageValue() * _owner.Stats.StrenghtModificator)
            : (int)(GetDamageValue() * _owner.Stats.IntelegenceModificator);

        private int GetDamageValue() => _nextAttackIsCritical ? critDamage : damage;

        private void TurnOnCriticalMode() => _nextAttackIsCritical = true;
        private void TurnOffCriticalMode() => _nextAttackIsCritical = false;
    }
}