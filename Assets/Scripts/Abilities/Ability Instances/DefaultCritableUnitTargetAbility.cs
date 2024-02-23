using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public abstract class DefaultCritableUnitTargetAbility : DefaultUnitTargetAbility, IAbleToHighlightAbilityButton, IAbleToDealAlternativeDamage, IAbleToReturnCurrentDamage
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

        public int CalculateAlternativeDamage() => Extensions.CalculateOutgoingDamageWithGameRules(criticalDamage, damageType, _owner.Stats);

        public int GetCalculatedCurrentDamage() => Extensions.CalculateOutgoingDamageWithGameRules(GetDefaultCurrentDamage(), damageType, _owner.Stats);

        public int GetDefaultCurrentDamage() => nextAttackIsCritical ? criticalDamage : damage;
    }
}