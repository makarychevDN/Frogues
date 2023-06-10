using UnityEngine;

namespace FroguesFramework
{
    public class BoxOnRatPassiveAbility : PassiveAbility
    {
        [SerializeField] private int blockValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.Health.IncreasePermanentArmor(blockValue);
            _owner.Animator.SetBool(CharacterAnimatorParameters.BoxIsOn, true);
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
            _owner.Health.OnBlockDestroyed.AddListener(() => _owner.Animator.SetBool(CharacterAnimatorParameters.BoxIsOn, false));
        }
    }
}