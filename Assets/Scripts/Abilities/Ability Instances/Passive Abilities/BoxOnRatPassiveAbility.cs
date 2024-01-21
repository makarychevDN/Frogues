using UnityEngine;

namespace FroguesFramework
{
    public class BoxOnRatPassiveAbility : PassiveAbility, IAbleToApplyArmor
    {
        [SerializeField] private int armorValue;

        public int CalculateArmor() => armorValue;

        public int GetDefaultArmorValue() => armorValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.Health.IncreasePermanentBlock(armorValue);
            _owner.Animator.SetBool(CharacterAnimatorParameters.BoxIsOn, true);
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
            _owner.Health.OnBlockDestroyed.AddListener(() => _owner.Animator.SetBool(CharacterAnimatorParameters.BoxIsOn, false));
        }
    }
}