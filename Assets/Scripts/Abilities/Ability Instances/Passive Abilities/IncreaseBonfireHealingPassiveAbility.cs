using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseBonfireHealingPassiveAbility : PassiveAbility
    {
        [SerializeField] private int additionalValue;

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
    }
}