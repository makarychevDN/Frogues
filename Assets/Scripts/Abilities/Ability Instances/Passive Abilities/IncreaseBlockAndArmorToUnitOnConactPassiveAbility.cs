using UnityEngine;

namespace FroguesFramework 
{ 
    public class IncreaseBlockAndArmorToUnitOnConactPassiveAbility : PassiveAbility, IAbleToApplyArmor, IAbleToApplyBlock
    {
        [SerializeField] private int blockValue;
        [SerializeField] private int armorValue;

        public int CalculateArmor() => armorValue;

        public int CalculateBlock() => Extensions.CalculateBlockWithGameRules(blockValue, _owner.Stats);

        public int GetDefaultArmorValue() => armorValue;

        public int GetDefaultBlockValue() => blockValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            unit.OnStepOnThisUnitByUnit.AddListener(IncreaseArmorAndBlockToUnit);
            unit.Movable.OnBumpIntoUnit.AddListener(IncreaseArmorAndBlockToUnit);
        }

        private void IncreaseArmorAndBlockToUnit(Unit unit)
        {
            unit.Health.IncreaseArmor(armorValue);
        }
    }
}