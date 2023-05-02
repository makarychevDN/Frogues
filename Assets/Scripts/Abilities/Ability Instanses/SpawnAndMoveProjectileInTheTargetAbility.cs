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
            EntryPoint.Instance.SpawnUnit(projectilePrefab, _owner, target.CurrentCell);
        }
    }
}
