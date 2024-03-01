using UnityEngine;

namespace FroguesFramework
{
    public class StrongShellPassiveAbility : PassiveAbility, IAbleToApplyArmor, IAbleToModifyMaxHP
    {
        [SerializeField] private int additionalArmor;
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
            _owner.Health.IncreaseArmor(additionalArmor);
        }

        public int CalculateArmor() => additionalArmor;

        public int GetDefaultArmorValue() => additionalArmor;

        public int GetModificatorForMaxHP() => healthCostValue;
    }
}