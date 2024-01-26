using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public abstract class DefaultCritableUnitTargetAbility : DefaultUnitTargetAbility, IAbleToHighlightAbilityButton, IAbleToDealAlternativeDamage, IAbleToReturnSingleValue
    {
        [SerializeField] private int criticalDamage;
        private bool nextAttackIsCritical;
        private UnityEvent<bool> _nextAttackIsCriticalStateEvent = new UnityEvent<bool>();

        protected void TurnOnCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(true);
            nextAttackIsCritical = true;
        }

        protected void TurnOffCriticalMode()
        {
            _nextAttackIsCriticalStateEvent.Invoke(false);
            nextAttackIsCritical = false;
        }

        public UnityEvent<bool> GetHighlightEvent() => _nextAttackIsCriticalStateEvent;

        public int GetDefaultAlternativeDamage() => criticalDamage;

        public DamageType GetAlternativeDamageType() => damageType;

        public int GetValue() => Extensions.CalculateIncomingDamageWithGameRules(damage, _owner.Stats);

        public int CalculateAlternativeDamage() => Extensions.CalculateIncomingDamageWithGameRules(criticalDamage, _owner.Stats);

        public override int CalculateDamage() => Extensions.CalculateIncomingDamageWithGameRules(nextAttackIsCritical ? criticalDamage : damage, _owner.Stats);
    }
}