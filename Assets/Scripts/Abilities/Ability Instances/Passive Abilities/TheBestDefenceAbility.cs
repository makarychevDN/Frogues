using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class TheBestDefenceAbility : NonTargetAbility, IAbleToApplySpikesModificator
    {
        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            _owner.Stats.AddStatEffect(new StatEffect(StatEffectTypes.spikes, _owner.AbilitiesManager.WeaponDamage, 1));
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public int GetdeltaOfSpikesValueForEachTurn() => 0;

        public bool GetSpikesEffectIsConstantly() => false;

        public int GetSpikesModificatorValue() => _owner.AbilitiesManager.WeaponDamage;

        public int GetTimeToEndOfSpikesEffect() => 1;
    }
}