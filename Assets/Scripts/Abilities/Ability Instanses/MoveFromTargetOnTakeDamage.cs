using UnityEngine;

namespace FroguesFramework
{
    public class MoveFromTargetOnTakeDamage : PassiveAbility
    {
        [SerializeField] private Unit target;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            unit.Health.OnApplyUnblockedDamage.AddListener(() => MoveFromTarget(target));
        }

        private void MoveFromTarget(Unit target)
        {

        }
    }
}