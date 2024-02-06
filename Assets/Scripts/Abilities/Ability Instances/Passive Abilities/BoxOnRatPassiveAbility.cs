using UnityEngine;

namespace FroguesFramework
{
    public class BoxOnRatPassiveAbility : PassiveAbility, IAbleToApplyArmor
    {
        [SerializeField] private int armorValue;
        [SerializeField] private RuntimeAnimatorController ownerWithoutBoxController;

        public int CalculateArmor() => armorValue;

        public int GetDefaultArmorValue() => armorValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.Health.IncreaseArmor(armorValue);
            _owner.Health.OnArmorDestroyed.AddListener(() => _owner.Animator.runtimeAnimatorController = ownerWithoutBoxController);
        }
    }
}