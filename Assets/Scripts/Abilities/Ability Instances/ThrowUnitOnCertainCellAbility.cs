using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class ThrowUnitOnCertainCellAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private Cell certainCell;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, _owner);
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }

            target.Movable.Move(certainCell, 20, 1);

        }
    }
}