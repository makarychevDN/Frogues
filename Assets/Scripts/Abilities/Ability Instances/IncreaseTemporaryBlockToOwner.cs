using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseTemporaryBlockToOwner : NonTargetAbility, IAbleToApplyBlock
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
            _owner.Health.IncreaseTemporaryBlock(CalculateBlock());
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);
        public int CalculateBlock() => Extensions.CalculateBlockWithGameRules(blockValue, _owner.Stats);
        public int GetDefaultBlockValue() => blockValue;
    }
}