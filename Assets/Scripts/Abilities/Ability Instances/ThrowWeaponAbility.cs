using UnityEngine;

namespace FroguesFramework
{
    public class ThrowWeaponAbility : SpawnAndMoveProjectileInTheTargetAbility, IAbleToReturnSingleValue, IAbleToReturnSecondSingleValue
    {
        [SerializeField] private int cooldownForLightWeapon;
        [SerializeField] private int cooldownForHeavyWeapon;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            SetupNewDataDueSelectedWeapon();
            _owner.AbilitiesManager.OnWeaponChanged.AddListener(SetupNewDataDueSelectedWeapon);
        }

        public override void UnInit()
        {
            _owner.AbilitiesManager.OnWeaponChanged.RemoveListener(SetupNewDataDueSelectedWeapon);
            base.UnInit();
        }

        public void SetupNewDataDueSelectedWeapon()
        {
            cooldownAfterUse = _owner.AbilitiesManager.WeaponActionPointsCost == 2 ? cooldownForHeavyWeapon : cooldownForLightWeapon;
            damageType = _owner.AbilitiesManager.DamageType;
        }

        public new int GetValue() => cooldownForLightWeapon;

        public int GetSecondValue() => cooldownForHeavyWeapon;
    }
}