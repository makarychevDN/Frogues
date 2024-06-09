using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility unitTargetAbilty;
        protected Unit _owner;

        public virtual void Act()
        {
            target = _owner.CurrentRoom.PlayableCharacters[0];

            unitTargetAbilty.PrepareToUsing(target);
            if (unitTargetAbilty.PossibleToUseOnUnit(target))
            {
                unitTargetAbilty.UseOnUnit(target);
                return;
            }

            _owner.MovementAbility.CalculateUsingArea();
            var theFirstCellOfPathAsList = new List<Cell> { 
                _owner.MovementAbility.SelectCells(new List<Cell> { target.CurrentCell })?.GetFirst() };

            if (!_owner.MovementAbility.PossibleToUseOnCells(theFirstCellOfPathAsList))
            {
                _owner.AbleToSkipTurn.AutoSkip();
                return;
            }

            _owner.MovementAbility.UseOnCells(theFirstCellOfPathAsList);
        }

        public virtual void Init(Unit owner)
        {
            _owner = owner;
        }
    }
}