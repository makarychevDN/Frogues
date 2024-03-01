using UnityEngine;

namespace FroguesFramework
{
    public class SelectUnitToBackStab : SelectUnitToCellAbility, IAbleToReturnSingleValue, IAbleToReturnSecondSingleValue
    {
        [SerializeField] protected int additionalCostForHeavyWeapon = 1;

        public override int GetDefaultDamage() => _owner.AbilitiesManager.WeaponDamage;

        public override DamageType GetDamageType() => damageType;

        public override int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(_owner.AbilitiesManager.WeaponDamage, damageType, _owner.Stats);

        public override int GetActionPointsCost()
        {
            return CalculateActionPointsCost;
        }

        protected override int CalculateActionPointsCost => _owner.AbilitiesManager.WeaponActionPointsCost + CurrentAdditionalCost;

        protected override void HashUnitToAreaTargetAbility(Unit unit)
        {
            ((IAbleToHashUnitTarget)areaTargetAbility).HashUnitTargetAndCosts(unit, CurrentAdditionalCost, CalculateBloodPointsCost, CalculateDamage());
        }

        public int GetValue() => 1;

        public int GetSecondValue() => 2 + additionalCostForHeavyWeapon;

        private int CurrentAdditionalCost => _owner.AbilitiesManager.WeaponActionPointsCost == 2 ? additionalCostForHeavyWeapon : 0;
    }
}