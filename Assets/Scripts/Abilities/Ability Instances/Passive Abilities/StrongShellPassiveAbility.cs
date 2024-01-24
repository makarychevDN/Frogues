using UnityEngine;

namespace FroguesFramework
{
    public class StrongShellPassiveAbility : PassiveAbility, IAbleToApplyArmor, IAbleToModifyMaxHP
    {
        [SerializeField] private int additionalConstantlyBlockValue;
        [SerializeField] private int healthCostValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseMaxHp(-healthCostValue);
            EntryPoint.Instance.OnNextRoomStarted.AddListener(AddConstantlyBlock);
        }

        public override void UnInit()
        {
            _owner.Health.IncreaseMaxHp(healthCostValue);
            EntryPoint.Instance.OnNextRoomStarted.RemoveListener(AddConstantlyBlock);
            base.UnInit();
        }

        private void AddConstantlyBlock()
        {
            _owner.Health.IncreaseArmor(additionalConstantlyBlockValue);
        }

        public int CalculateArmor() => additionalConstantlyBlockValue;

        public int GetDefaultArmorValue() => additionalConstantlyBlockValue;

        public int GetModificatorForMaxHP() => healthCostValue;
    }
}