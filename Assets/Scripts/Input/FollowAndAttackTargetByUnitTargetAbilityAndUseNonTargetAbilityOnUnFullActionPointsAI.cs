using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnUnFullActionPointsAI : FollowAndAttackTargetByUnitTargetAbilityAI
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;
        private bool _needToActAsBase;

        public override void Act()
        {
            if (!_owner.ActionPoints.Full && !_needToActAsBase)
            {
                nonTargetAbility.Use();
                _owner.AbleToSkipTurn.AutoSkip();
                return;
            }

            _needToActAsBase = true;
            base.Act();
        }

        public override void Init(Unit owner)
        {
            base.Init(owner);

            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(() => _needToActAsBase = false);
        }
    }
}