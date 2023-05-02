using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageOnContactWithUnitPassiveAbility : PassiveAbility
    {
        [SerializeField] private int damage;
        public override void Init(Unit unit)
        {
            base.Init(unit);

            unit.OnStepOnThisUnitByTheUnit.AddListener(DealDamage);
            unit.Movable.OnBumpIntoUnit.AddListener(DealDamage);
        }

        private void DealDamage(Unit unit)
        {
            unit.Health.TakeDamage(damage);
        }
    }
}

