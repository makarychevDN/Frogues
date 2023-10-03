using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private Unit projectilePrefab;
        [SerializeField] private AudioSource onProjectileContactWithTargetSound;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            var projectile = EntryPoint.Instance.SpawnUnit(projectilePrefab, _owner, target.CurrentCell);
            projectile.Movable.OnBumpIntoUnit.AddListener(DealDamage);
        }

        private void DealDamage(Unit target)
        {
            target.Health.TakeDamage(CalculateDamage, ignoreArmor, _owner);
            onProjectileContactWithTargetSound.Play();
        }
    }
}
