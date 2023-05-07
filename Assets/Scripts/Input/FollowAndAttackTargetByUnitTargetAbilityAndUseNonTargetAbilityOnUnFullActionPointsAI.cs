using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnUnFullActionPointsAI : FollowAndAttackTargetByUnitTargetAbilityAI
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;

        public override void Act()
        {
            if (!_unit.ActionPoints.Full)
            {
                nonTargetAbility.Use();
                return;
            }

            base.Act();
        }
    }
}