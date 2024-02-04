using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : DefaultUnitTargetAbility, IAbleToApplyBlock, IAbleToApplyArmor
    {
        [SerializeField] private Unit projectilePrefab;
        [SerializeField] private AudioSource onProjectileContactWithTargetSound;
        [SerializeField] protected int additionalBlockToTarget;
        [SerializeField] protected int additionalArmorToTarget;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            var projectile = EntryPoint.Instance.SpawnUnit(projectilePrefab, _owner, target.CurrentCell);
            projectile.Movable.OnBumpIntoUnit.AddListener(DealDamage);
        }

        private void DealDamage(Unit target)
        {
            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, _owner);
            target.Health.IncreaseBlock(CalculateBlock());
            target.Health.IncreaseArmor(CalculateArmor());

            if(onProjectileContactWithTargetSound != null)
                onProjectileContactWithTargetSound.Play();
            
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect,effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }
        }

        public int GetDefaultBlockValue() => additionalArmorToTarget;

        public int CalculateBlock() => Extensions.CalculateBlockWithGameRules(additionalBlockToTarget, _owner.Stats);

        public int GetDefaultArmorValue() => additionalArmorToTarget;

        public int CalculateArmor() => additionalArmorToTarget;
    }
}
