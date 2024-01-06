using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikeAbility : NonTargetAbility, IAbleToApplySpikesModificator
    {
        [Space, Header("Ability Settings")]
        [SerializeField] private int spikesValue;
        [SerializeField] private int deltaValue;
        [SerializeField] private int timeToEndEffect = 1;
        [SerializeField] private bool effectIsConstantly;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurrentlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            _owner.Stats.AddStatEffect(new StatEffect(StatEffectTypes.spikes, spikesValue, timeToEndEffect,
                effectIsConstantly: effectIsConstantly));
        }

        private void RemoveCurrentlyActive() => CurrentlyActiveObjects.Remove(this);

        public int GetdeltaOfSpikesValueForEachTurn() => deltaValue;

        public bool GetSpikesEffectIsConstantly() => effectIsConstantly;

        public int GetSpikesModificatorValue() => spikesValue;

        public int GetTimeToEndOfSpikesEffect() => timeToEndEffect;
    }
}