using UnityEngine;

namespace FroguesFramework
{
    public class FencingMasterPassiveAbility : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int additinalPercentage;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            MultiplyDamage();
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.AddListener(MultiplyDamage);
        }

        public override void UnInit()
        {
            _owner.AbilitiesManager.SetWeaponDamage((_owner.AbilitiesManager.WeaponDamage / CalculateMulitplier(additinalPercentage)).RoundWithGameRules());
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.RemoveListener(MultiplyDamage);
            base.UnInit();
        }

        private void MultiplyDamage()
        {
            _owner.AbilitiesManager.SetWeaponDamage((_owner.AbilitiesManager.WeaponDamage * CalculateMulitplier(additinalPercentage)).RoundWithGameRules());
        }

        private float CalculateMulitplier(int value) => value * 0.01f + 1;

        public int GetValue() => additinalPercentage;
    }
}