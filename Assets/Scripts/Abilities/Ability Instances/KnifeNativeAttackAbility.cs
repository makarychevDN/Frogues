using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class KnifeNativeAttackAbility : DefaultUnitTargetAbility, IAbleToHighlightAbilityButton
    {
        [SerializeField] private int critDamage;
        private bool _nextAttackIsCritical;
        private UnityEvent<bool> _nextAttackIsCriticalStateEvent = new UnityEvent<bool>();

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.AddListener(TurnOnCriticalMode);
        }

        public override void UnInit()
        {
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.RemoveListener(TurnOnCriticalMode);
            base.UnInit();
        }

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            TurnOffCriticalMode();
            return base.ApplyEffect(time, target);
        }

        protected override int CalculateDamage => damageType == DamageType.physics
            ? (int)(GetDamageValue() * _owner.Stats.StrenghtModificator)
            : (int)(GetDamageValue() * _owner.Stats.IntelegenceModificator);

        private int GetDamageValue() => _nextAttackIsCritical ? critDamage : damage;

        private void TurnOnCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(true);
            _nextAttackIsCritical = true;
            ignoreArmor = true;
        }

        private void TurnOffCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(false);
            _nextAttackIsCritical = false;
            ignoreArmor = false;
        }

        public UnityEvent<bool> GetHighlightEvent() => _nextAttackIsCriticalStateEvent;
    }
}