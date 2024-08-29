using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageAroundOwnerNonTargetAbility : NonTargetAbility, IAbleToDealDamage, IAbleToReturnRange, IAbleToApplyStrenghtModificator,
        IAbleToApplyIntelligenceModificator, IAbleToApplySpikesModificator, IAbleToApplyImmobilizedModificator
    {
        [SerializeField] protected int radius;
        [SerializeField] protected int damage;
        [SerializeField] protected DamageType damageType;
        [SerializeField] protected List<StatEffect> additionalDebuffs;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if(impactSoundSource != null)
                impactSoundSource.Play();

            _owner.CurrentRoom.CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            _owner.CurrentRoom.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radius, true, false)
                .Where(cell => cell.Content != null).ToList()
                .ForEach(cell => 
                { 
                    if(CalculateDamage() != 0)
                        cell.Content.Health.TakeDamage(CalculateDamage(), null);
                    
                    foreach(StatEffect effect in additionalDebuffs)
                    {
                        cell.Content.Stats.AddStatEffect(new StatEffect(effect));
                    }
                });
        }

        private void RemoveCurremtlyActive() => _owner.CurrentRoom.CurrentlyActiveObjects.Remove(this);

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;

        public virtual int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(damage, damageType, _owner.Stats);

        public int ReturnRange() => radius;        

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.strength);

        public int GetDeltaOfStrenghtValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.strength);

        public int GetTimeToEndOfStrenghtEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.strength);

        public bool GetStrenghtEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.strength);
        #endregion

        #region IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.intelligence);

        public int GetDeltaOfIntelligenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.intelligence);

        public int GetTimeToEndOfIntelligenceEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.intelligence);

        public bool GetIntelligenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.intelligence);
        #endregion

        #region IAbleToApplySpikesModificator
        public int GetSpikesModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.thorns);

        public int GetdeltaOfSpikesValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.thorns);

        public int GetTimeToEndOfSpikesEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.thorns);

        public bool GetSpikesEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.thorns);
        #endregion

        #region IAbleToApplyImmobilizedModificator
        public int GetTimeToEndOfImmpobilizedEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.thorns);
        #endregion
    }
}