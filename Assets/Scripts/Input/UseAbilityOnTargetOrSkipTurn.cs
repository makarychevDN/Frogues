using UnityEngine;

namespace FroguesFramework
{
    public class UseAbilityOnTargetOrSkipTurn : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility unitTargetAbilty;
        private Unit _owner;

        public void Act()
        {
            target = _owner.CurrentRoom.PlayableCharacters[0];
            unitTargetAbilty.PrepareToUsing(target);
            if (unitTargetAbilty.PossibleToUseOnUnit(target))
            {
                unitTargetAbilty.UseOnUnit(target);
                return;
            }

            _owner.AbleToSkipTurn.AutoSkip();
        }

        public void Init(Unit owner)
        {
            _owner = owner;
        }
    }
}