using UnityEngine;

namespace FroguesFramework
{
    public class FencingMasterPassiveAbility : PassiveAbility
    {
        [SerializeField] private float weaponsDamageMultiplier;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            MultiplyDamage();
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.AddListener(MultiplyDamage);
        }

        public override void UnInit()
        {
            _owner.AbilitiesManager.SetWeaponDamage((_owner.AbilitiesManager.WeaponDamage / weaponsDamageMultiplier).RoundWithGameRules());
            _owner.AbilitiesManager.OnWeaponsDamageUpdated.RemoveListener(MultiplyDamage);
            base.UnInit();
        }

        private void MultiplyDamage()
        {
            _owner.AbilitiesManager.SetWeaponDamage((_owner.AbilitiesManager.WeaponDamage * weaponsDamageMultiplier).RoundWithGameRules());
        }
    }
}