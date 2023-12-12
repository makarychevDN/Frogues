using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class KnifeNativeAttackAbility : DefaultUnitTargetAbility, IAbleToHighlightAbilityButton
    {
        [SerializeField] private int critDamage;
        private UnityEvent<bool> _nextAttackIsCriticalStateEvent = new UnityEvent<bool>();

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.AddListener(TurnOnCriticalMode);
            OnEffectApplied.AddListener(TurnOffCriticalMode);
        }

        public override void UnInit()
        {
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.RemoveListener(TurnOnCriticalMode);
            OnEffectApplied.RemoveListener(TurnOffCriticalMode);
            base.UnInit();
        }

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            return base.ApplyEffect(time, target);
        }

        public override int CalculateDamage() => Extensions.CalculateDamageWithGameRules(GetDamageValue(), damageType, _owner.Stats);

        private int GetDamageValue() => _owner.AbilitiesManager.WeaponDamage; 

        private void TurnOnCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(true);
            ignoreArmor = true;
            _owner.AbilitiesManager.SetWeaponDamage(critDamage);
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.Invoke();
        }

        private void TurnOffCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(false);
            ignoreArmor = false;
            _owner.AbilitiesManager.SetWeaponDamage(damage);
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.Invoke();
        }

        public UnityEvent<bool> GetHighlightEvent() => _nextAttackIsCriticalStateEvent;
    }
}