using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public abstract class BattleStanceAbility : NonTargetAbility, IAbleToHighlightAbilityButton
    {
        [SerializeField] protected bool stanceActiveNow;
        public UnityEvent<BattleStanceAbility> OnThisStanceSelected;
        public UnityEvent<bool> HighlightButtonEvent;

        public override void Use()
        {
            if (!IsResoursePointsEnough())
                return;

            SpendResourcePoints();

            var updatedValue = !stanceActiveNow;
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffectWithDelay(timeBeforeImpact, updatedValue));   
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        public virtual IEnumerator ApplyEffectWithDelay(float time, bool isActive)
        {
            yield return new WaitForSeconds(time);
            ApplyEffect(isActive);
        }

        public virtual void ApplyEffect(bool isActive)
        {
            stanceActiveNow = isActive;
            HighlightButtonEvent.Invoke(stanceActiveNow);

            if (isActive)
            {
                OnThisStanceSelected.Invoke(this);
            }
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public UnityEvent<bool> GetHighlightEvent() => HighlightButtonEvent;

        public bool StanceActiveNow => stanceActiveNow;
    }
}