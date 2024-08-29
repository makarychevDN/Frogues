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

            _owner.CurrentRoom.CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            _owner.Health.IncreaseBlock(CalculateBlock());
        }

        private void RemoveCurremtlyActive() => _owner.CurrentRoom.CurrentlyActiveObjects.Remove(this);
        public int CalculateBlock() => blockValue;
        public int GetDefaultBlockValue() => blockValue;
    }
}