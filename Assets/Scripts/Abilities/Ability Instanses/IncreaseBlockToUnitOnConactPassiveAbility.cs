using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework 
{ 
    public class IncreaseBlockToUnitOnConactPassiveAbility : PassiveAbility
    {
        [SerializeField] private int blockValue;
        public override void Init(Unit unit)
        {
            base.Init(unit);

            unit.OnStepOnThisUnitByUnit.AddListener(IncreaseBlockToUnit);
            unit.Movable.OnBumpIntoUnit.AddListener(IncreaseBlockToUnit);
        }

        private void IncreaseBlockToUnit(Unit unit)
        {
            unit.Health.IncreaseTemporaryArmor(blockValue);
        }
    }
}