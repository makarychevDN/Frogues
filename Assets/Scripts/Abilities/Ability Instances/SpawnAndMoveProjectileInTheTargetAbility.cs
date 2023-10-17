using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private Unit projectilePrefab;
        [SerializeField] private AudioSource onProjectileContactWithTargetSound;
        [SerializeField] private List<StatEffect> addtionalDebufs;

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
            
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.effectIsConstantly));
            }
        }
    }
}
