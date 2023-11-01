using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public abstract class BattleStanceAbility : NonTargetAbility, IAbleToHighlightAbilityButton
    {
        protected bool stanceActiveNow;
        public UnityEvent<BattleStanceAbility> OnThisStanceSelected;
        public UnityEvent<bool> OnActiveNowUpdated;

        public override void Use()
        {
            if (!IsResoursePointsEnough())
                return;

            SpendResourcePoints();
            stanceActiveNow = !stanceActiveNow;
            OnActiveNowUpdated.Invoke(stanceActiveNow);

            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffectWithDelay(timeBeforeImpact, stanceActiveNow));   
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        public virtual IEnumerator ApplyEffectWithDelay(float time, bool isActive)
        {
            yield return new WaitForSeconds(time);
            ApplyEffect(isActive);
        }

        public virtual void ApplyEffect(bool isActive)
        {
            if (isActive)
            {
                OnThisStanceSelected.Invoke(this);
            }
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public UnityEvent<bool> GetHighlightEvent() => OnActiveNowUpdated;
    }
}