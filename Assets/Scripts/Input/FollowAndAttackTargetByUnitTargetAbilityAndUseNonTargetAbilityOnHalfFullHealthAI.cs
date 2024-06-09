using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnHalfFullHealthAI : FollowAndAttackTargetByUnitTargetAbilityAI, IAbleToAct
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;

        public override void Act()
        {
            if (_owner.Health.CurrentHp < _owner.Health.MaxHp * 0.5f)
            {
                nonTargetAbility.Use();
                return;
            }

            base.Act();
        }
    }
}