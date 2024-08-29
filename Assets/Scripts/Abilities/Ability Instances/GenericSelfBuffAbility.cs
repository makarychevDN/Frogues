using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class GenericSelfBuffAbility : NonTargetAbility, IAbleToApplyBlock, IAbleToApplyArmor, IAbleToApplyActionPointsRegenerationPenalty
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