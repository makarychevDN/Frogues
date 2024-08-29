using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : DefaultUnitTargetAbility, IAbleToApplyArmor, IAbleToReturnSingleValue
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
            var projectile = Extensions.SpawnUnit(projectilePrefab, _owner, target.CurrentCell, _owner.CurrentRoom);
            projectile.Movable.OnBumpIntoUnit.AddListener(DealDamage);
        }

        private void DealDamage(Unit target)
        {
            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, countsAsAttack ? _owner : null);
            if (CalculateArmor() != 0) target.Health.IncreaseArmor(CalculateArmor());
            if (additionalTemporaryActionPointsToTarget != 0) target.ActionPoints.IncreaseTemporaryPoints(additionalTemporaryActionPointsToTarget);

            if(onProjectileContactWithTargetSound != null)
                onProjectileContactWithTargetSound.Play();           
        }

        public int GetDefaultBlockValue() => additionalBlockToTarget;

        public int GetDefaultArmorValue() => additionalArmorToTarget;

        public int CalculateArmor() => additionalArmorToTarget;

        public int GetValue() => additionalTemporaryActionPointsToTarget;
    }
}
