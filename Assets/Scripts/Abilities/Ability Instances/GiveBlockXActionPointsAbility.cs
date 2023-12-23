using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class GiveBlockXActionPointsAbility : NonTargetAbility, IAbleToApplyBlock, IAbleToApplyArmor, IAbleToHaveDelta, IAbleToHaveAlternativeDelta
    {
        [SerializeField] private int blockValue;
        [SerializeField] private int armorValue;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            _owner.Health.IncreaseTemporaryBlock(CalculateBlock());
            _owner.Health.IncreasePermanentBlock(CalculateArmor());
            SpendResourcePoints();

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

        public override bool PossibleToUse()
        {
            return _owner.ActionPoints.CurrentPoints > 0;
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public int GetDefaultBlockValue() => blockValue * CalculateActionPointsCost;

        public int CalculateBlock() => Extensions.CalculateBlockWithGameRules(blockValue * CalculateActionPointsCost, _owner.Stats);

        public int GetDefaultArmorValue() => CalculateActionPointsCost * armorValue;

        public int CalculateArmor() => CalculateActionPointsCost * armorValue;

        public int GetDeltaValue() => blockValue;

        public int GetStepValue() => 1;

        public int GetAlternativeDeltaValue() => armorValue;

        public int GetAlternativeStepValue() => 1;

        protected override int CalculateActionPointsCost => _owner.ActionPoints.CurrentPoints;

        public override int GetActionPointsCost() => CalculateActionPointsCost;
    }
}