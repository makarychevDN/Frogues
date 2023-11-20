using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class GiveBlockXActionPointsAbility : NonTargetAbility
    {
        [SerializeField] private int blockValue;
        [SerializeField] private int armorValue;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            _owner.Health.IncreaseTemporaryBlock(blockValue * _owner.ActionPoints.CurrentPoints);
            _owner.Health.IncreasePermanentBlock(armorValue * _owner.ActionPoints.CurrentPoints);
            SpendResourcePoints();

            CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            _owner.Health.IncreaseTemporaryBlock(Extensions.CalculateBlockWithGameRules(blockValue, _owner.Stats));
        }

        public override bool PossibleToUse()
        {
            return _owner.ActionPoints.CurrentPoints > 0;
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        protected override int CalculateActionPointsCost => _owner.ActionPoints.CurrentPoints;
    }
}