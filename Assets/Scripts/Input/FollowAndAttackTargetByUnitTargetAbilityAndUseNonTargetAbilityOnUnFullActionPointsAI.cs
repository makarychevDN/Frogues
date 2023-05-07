using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnUnFullActionPointsAI : FollowAndAttackTargetByUnitTargetAbilityAI
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;
        private bool _needToActAsBase;

        public override void Act()
        {
            if (!_unit.ActionPoints.Full && !_needToActAsBase)
            {
                nonTargetAbility.Use();
                _unit.AbleToSkipTurn.AutoSkip();
                return;
            }

            _needToActAsBase = true;
            base.Act();
        }

        public override void Init()
        {
            base.Init();

            _unit.AbleToSkipTurn.OnSkipTurn.AddListener(() => _needToActAsBase = false);
        }
    }
}