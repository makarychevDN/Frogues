using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnHalfFullHealthAI : FollowAndAttackTargetByUnitTargetAbilityAI, IAbleToAct
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;

        public override void Act()
        {
            if (_unit.Health.CurrentHp < _unit.Health.MaxHp * 0.5f)
            {
                nonTargetAbility.Use();
                return;
            }

            base.Act();
        }
    }
}