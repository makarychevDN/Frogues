using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class ReliableStrike : DefaultUnitTargetAbility
    {
        [SerializeField] private int blockValue;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(CalculateDamage, ignoreArmor, _owner);
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.effectIsConstantly));
            }

            _owner.Health.IncreaseTemporaryBlock(Extensions.CalculateBlockWithGameRules(blockValue, _owner.Stats));
            OnEffectApplied.Invoke();
        }
    }
}