using UnityEngine;

namespace FroguesFramework
{
    public class StrongBiteAbility : DefaultUnitTargetAbility, IAbleToHaveDelta
    {
        [SerializeField] private int healthDeltaStep;
        [SerializeField] private int damageValueForEachStep;

        public override int CalculateDamage()
        {
            return _owner.Health.MaxHp / healthDeltaStep * damageValueForEachStep;
        }

        public int GetDeltaValue() => healthDeltaStep;

        public int GetStepValue() => damageValueForEachStep;
    }
}