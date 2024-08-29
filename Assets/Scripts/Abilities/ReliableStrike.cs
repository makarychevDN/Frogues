using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class ReliableStrike : DefaultUnitTargetAbility, IAbleToApplyBlock
    {
        [SerializeField] private int blockValue;

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, _owner);

            _owner.Health.IncreaseBlock(CalculateBlock());
            OnEffectApplied.Invoke();
        }


        public int CalculateBlock() => blockValue;
        public int GetDefaultBlockValue() => blockValue;
    }
}