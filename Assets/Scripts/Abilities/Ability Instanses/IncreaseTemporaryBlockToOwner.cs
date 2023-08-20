using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseTemporaryBlockToOwner : NonTargetAbility
    {
        [SerializeField] private int blockValue;

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
            _owner.Health.IncreaseTemporaryBlock(blockValue);
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);
    }
}