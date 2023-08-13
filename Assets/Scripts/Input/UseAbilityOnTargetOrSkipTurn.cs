using UnityEngine;

namespace FroguesFramework
{
    public class UseAbilityOnTargetOrSkipTurn : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility unitTargetAbilty;
        private Unit _unit;

        public void Act()
        {
            unitTargetAbilty.PrepareToUsing(target);
            if (unitTargetAbilty.PossibleToUseOnUnit(target))
            {
                unitTargetAbilty.UseOnUnit(target);
                return;
            }

            _unit.AbleToSkipTurn.AutoSkip();
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}