using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseBonfireHealingPassiveAbility : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int additionalValue = 7;

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

        public int GetValue() => additionalValue;

    }
}