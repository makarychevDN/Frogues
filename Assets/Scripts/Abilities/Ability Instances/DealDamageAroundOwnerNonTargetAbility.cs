using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageAroundOwnerNonTargetAbility : NonTargetAbility, IAbleToDealDamage, IAbleToReturnRange, IAbleToApplyStrenghtModificator,
        IAbleToApplyIntelligenceModificator, IAbleToApplyDexterityModificator, IAbleToApplyDefenceModificator,
        IAbleToApplySpikesModificator, IAbleToApplyImmobilizedModificator
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

            CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radius, true, false)
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

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;

        public int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(damage, damageType, _owner.Stats);

        public int ReturnRange() => radius;
        
        #region IAbleToApplyDefenceModificator
        public int GetDefenceModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.defence);

        public int GetdeltaOfDefenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.defence);

        public int GetTimeToEndOfDefenceEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.defence);

        public bool GetDefenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.defence);
        #endregion

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.strenght);

        public int GetDeltaOfStrenghtValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.strenght);

        public int GetTimeToEndOfStrenghtEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.strenght);

        public bool GetStrenghtEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.strenght);
        #endregion

        #region IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.intelegence);

        public int GetDeltaOfIntelligenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.intelegence);

        public int GetTimeToEndOfIntelligenceEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.intelegence);

        public bool GetIntelligenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.intelegence);
        #endregion

        #region IAbleToApplyDexterityModificator
        public int GetDexterityModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.dexterity);

        public int GetDeltaOfDexterityValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.dexterity);

        public int GetTimeToEndOfDexterityEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.dexterity);

        public bool GetDexterityEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.dexterity);
        #endregion

        #region IAbleToApplySpikesModificator
        public int GetSpikesModificatorValue() => Extensions.GetModificatorValue(additionalDebuffs, StatEffectTypes.spikes);

        public int GetdeltaOfSpikesValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(additionalDebuffs, StatEffectTypes.spikes);

        public int GetTimeToEndOfSpikesEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.spikes);

        public bool GetSpikesEffectIsConstantly() => Extensions.GetEffectIsConstantly(additionalDebuffs, StatEffectTypes.spikes);
        #endregion

        #region IAbleToApplyImmobilizedModificator
        public int GetTimeToEndOfImmpobilizedEffect() => Extensions.GetTimeToEndOfEffect(additionalDebuffs, StatEffectTypes.spikes);
        #endregion
    }
}