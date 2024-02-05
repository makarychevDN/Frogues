using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnUnFullHealthAI : FollowAndAttackTargetByUnitTargetAbilityAI, IAbleToAct
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;

        public override void Act()
        {
            if (!_unit.Health.Full)
            {
                nonTargetAbility.Use();
                return;
            }

            base.Act();
        }
    }
}