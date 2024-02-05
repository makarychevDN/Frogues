using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : DefaultUnitTargetAbility, IAbleToApplyBlock, IAbleToApplyArmor, IAbleToReturnSingleValue
    {
        [SerializeField] private Unit projectilePrefab;
        [SerializeField] private AudioSource onProjectileContactWithTargetSound;
        [SerializeField] protected int additionalBlockToTarget;
        [SerializeField] protected int additionalArmorToTarget;
        [SerializeField] protected int additionalTemporaryActionPointsToTarget;
        [SerializeField] private bool countsAsAttack = true;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            var projectile = EntryPoint.Instance.SpawnUnit(projectilePrefab, _owner, target.CurrentCell);
            projectile.Movable.OnBumpIntoUnit.AddListener(DealDamage);
        }

        private void DealDamage(Unit target)
        {
            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, countsAsAttack ? _owner : null);
            if (CalculateBlock() != 0) target.Health.IncreaseBlock(CalculateBlock());
            if (CalculateArmor() != 0) target.Health.IncreaseArmor(CalculateArmor());
            if (additionalTemporaryActionPointsToTarget != 0) target.ActionPoints.IncreaseTemporaryPoints(additionalTemporaryActionPointsToTarget);

            if(onProjectileContactWithTargetSound != null)
                onProjectileContactWithTargetSound.Play();
            
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect,effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }
        }

        public int GetDefaultBlockValue() => additionalBlockToTarget;

        public int CalculateBlock() => Extensions.CalculateBlockWithGameRules(additionalBlockToTarget, _owner.Stats);

        public int GetDefaultArmorValue() => additionalArmorToTarget;

        public int CalculateArmor() => additionalArmorToTarget;

        public int GetValue() => additionalTemporaryActionPointsToTarget;
    }
}
