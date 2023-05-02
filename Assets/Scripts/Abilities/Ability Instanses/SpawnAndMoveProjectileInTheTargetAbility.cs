using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private Unit projectilePrefab;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.Init();
            projectile.CurrentCell = _owner.CurrentCell;
            projectile.Movable.Move(target.CurrentCell, false);
        }
    }
}
