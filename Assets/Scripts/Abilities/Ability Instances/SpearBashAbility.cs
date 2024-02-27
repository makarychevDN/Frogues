using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpearBashAbility : DefaultUnitTargetAbility, IAbleToDealAlternativeDamage
    {
        [SerializeField] private int damageForTooCloseToTargetAttack;

        public int CalculateAlternativeDamage() => Extensions.CalculateOutgoingDamageWithGameRules(damageForTooCloseToTargetAttack, damageType, _owner.Stats);

        public DamageType GetAlternativeDamageType() => damageType;

        public int GetDefaultAlternativeDamage() => damageForTooCloseToTargetAttack;

        private int CalculateDamageForTargetDueDistance(Unit target)
        {
            return target.CurrentCell.DistanceToCell(_owner.CurrentCell) == 1 ? CalculateAlternativeDamage() : CalculateDamage();
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (target != null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(CalculateDamageForTargetDueDistance(target), ignoreArmor);
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position);
        }

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(CalculateDamageForTargetDueDistance(target), ignoreArmor, _owner);
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }

            OnEffectApplied.Invoke();
        }
    }
}