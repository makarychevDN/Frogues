using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class GenericSelfBuffAbility : NonTargetAbility, IAbleToApplyStrenghtModificator,
        IAbleToApplySpikesModificator, IAbleToApplyImmobilizedModificator, IAbleToApplyBlock, IAbleToApplyArmor, IAbleToApplyActionPointsRegenerationPenalty

    {
        [Space, Header("Ability Settings")] 
        [SerializeField] private int temporaryBlockValue;
        [SerializeField] private int permanentBlockValue;
        [SerializeField] private int actionPointsRegenerationPenalty;
        [SerializeField] private List<StatEffect> effects;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            _owner.CurrentRoom.CurrentlyActiveObjects.Add(this);

            if(healthCost == 0)
                _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            _owner.ActionPoints.IncreasePenaltyToRegeneration(actionPointsRegenerationPenalty);

            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurrentlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);

            if (temporaryBlockValue != 0)
                _owner.Health.IncreaseBlock(temporaryBlockValue);
            if (permanentBlockValue != 0)
                _owner.Health.IncreaseArmor(permanentBlockValue);

            foreach (StatEffect buff in effects)
                _owner.Stats.AddStatEffect(new StatEffect(buff));
        }

        private void RemoveCurrentlyActive() => _owner.CurrentRoom.CurrentlyActiveObjects.Remove(this);

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => Extensions.GetModificatorValue(effects, StatEffectTypes.strength);

        public int GetDeltaOfStrenghtValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(effects, StatEffectTypes.strength);

        public int GetTimeToEndOfStrenghtEffect() => Extensions.GetTimeToEndOfEffect(effects, StatEffectTypes.strength);

        public bool GetStrenghtEffectIsConstantly() => Extensions.GetEffectIsConstantly(effects, StatEffectTypes.strength);
        #endregion

        #region IAbleToApplySpikesModificator
        public int GetSpikesModificatorValue() => Extensions.GetModificatorValue(effects, StatEffectTypes.thorns);

        public int GetdeltaOfSpikesValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(effects, StatEffectTypes.thorns);

        public int GetTimeToEndOfSpikesEffect() => Extensions.GetTimeToEndOfEffect(effects, StatEffectTypes.thorns);

        public bool GetSpikesEffectIsConstantly() => Extensions.GetEffectIsConstantly(effects, StatEffectTypes.thorns);
        #endregion

        #region IAbleToApplyImmobilizedModificator
        public int GetTimeToEndOfImmpobilizedEffect() => Extensions.GetTimeToEndOfEffect(effects, StatEffectTypes.thorns);
        #endregion

        #region IAbleToApplyBlock
        public int GetDefaultBlockValue() => temporaryBlockValue;

        public int CalculateBlock() => temporaryBlockValue;
        #endregion

        #region IAbleToApplyArmor
        public int GetDefaultArmorValue() => permanentBlockValue;

        public int CalculateArmor() => permanentBlockValue;
        #endregion

        public int GetActionPointsRegenerationPenaltyValue() => actionPointsRegenerationPenalty;
    }
}