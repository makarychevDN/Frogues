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
            RecalculateCooldown();
            _owner.AbilitiesManager.OnWeaponChanged.AddListener(RecalculateCooldown);
        }

        public override void UnInit()
        {
            _owner.AbilitiesManager.OnWeaponChanged.RemoveListener(RecalculateCooldown);
            base.UnInit();
        }

        public void RecalculateCooldown()
        {
            cooldownAfterUse = _owner.AbilitiesManager.WeaponActionPointsCost == 2 ? cooldownForHeavyWeapon : cooldownForLightWeapon;
        }

        public int GetValue() => cooldownForLightWeapon;

        public int GetSecondValue() => cooldownForHeavyWeapon;
    }
}