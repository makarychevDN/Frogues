using System.Collections;
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

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(GetCalculatedCurrentDamage(), ignoreArmor, _owner);
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }

            OnEffectApplied.Invoke();
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (target != null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(GetCalculatedCurrentDamage(), ignoreArmor);
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position);
        }
    }
}