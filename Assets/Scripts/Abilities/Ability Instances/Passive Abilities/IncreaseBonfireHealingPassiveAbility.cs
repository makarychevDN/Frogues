using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseBonfireHealingPassiveAbility : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private float additionalValue = 0.33f;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            EntryPoint.Instance.IncreaseBonfireHealingValue(additionalValue);
        }

        public override void UnInit()
        {
            EntryPoint.Instance.IncreaseBonfireHealingValue(-additionalValue);
            base.UnInit();
        }

        public int GetValue() => (additionalValue * 100).RoundWithGameRules();

    }
}