using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class KnifeNativeAttackAbility : DefaultUnitTargetAbility, IAbleToHighlightAbilityButton, IAbleToDealAlternativeDamage, IAbleToReturnCurrentDamage
    {
        [SerializeField] private int critDamage;
        private bool nextAttackIsCritical;
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
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, _owner);
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }

            OnEffectApplied.Invoke();
        }

        private void TurnOnCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(true);
            nextAttackIsCritical = true;
            _owner.AbilitiesManager.SetWeaponDamage(critDamage);
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.Invoke();
        }

        private void TurnOffCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(false);
            
            nextAttackIsCritical = false;
            _owner.AbilitiesManager.SetWeaponDamage(damage);
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.Invoke();
        }

        public UnityEvent<bool> GetHighlightEvent() => _nextAttackIsCriticalStateEvent;

        public int GetDefaultAlternativeDamage() => critDamage;
        public override int CalculateDamage() => Extensions.CalculateDamageWithGameRules(damage, damageType, _owner.Stats);
        public DamageType GetAlternativeDamageType() => damageType;
        public int CalculateAlternativeDamage() => Extensions.CalculateDamageWithGameRules(critDamage, damageType, _owner.Stats);
        public int GetCalculatedCurrentDamage() => nextAttackIsCritical ? CalculateAlternativeDamage() : CalculateDamage();
        public int GetDefaultCurrentDamage() => nextAttackIsCritical ? critDamage : damage;
    }
}