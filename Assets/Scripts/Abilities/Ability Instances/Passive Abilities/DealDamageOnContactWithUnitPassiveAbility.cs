using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageOnContactWithUnitPassiveAbility : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int damage;

        public int GetValue() => damage;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            unit.OnStepOnThisUnitByUnit.AddListener(DealDamage);
            unit.Movable.OnBumpIntoUnit.AddListener(DealDamage);
        }

        public override void UnInit()
        {
            _owner.OnStepOnThisUnitByUnit.RemoveListener(DealDamage);
            _owner.Movable.OnBumpIntoUnit.RemoveListener(DealDamage);

            base.UnInit();
        }

        private void DealDamage(Unit unit)
        {
            unit.Health.TakeDamage(damage, null);
        }
    }
}

