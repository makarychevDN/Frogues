using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikeAbility : NonTargetAbility
    {
        [Space, Header("Ability Settings")]
        [SerializeField] private int spikesValue;
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
    }
}