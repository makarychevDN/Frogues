using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility unitTargetAbilty;
        protected Unit _unit;

        public virtual void Act()
        {
            if (unitTargetAbilty.PossibleToUseOnUnit(target))
            {
                unitTargetAbilty.UseOnUnit(target);
                return;
            }

            _unit.MovementAbility.CalculateUsingArea();
            var theFirstCellOfPathAsList = new List<Cell> { 
                _unit.MovementAbility.SelectCells(new List<Cell> { target.CurrentCell })?.GetFirst() };

            if (!_unit.MovementAbility.PossibleToUseOnCells(theFirstCellOfPathAsList))
            {
                _unit.AbleToSkipTurn.AutoSkip();
                return;
            }

            _unit.MovementAbility.UseOnCells(theFirstCellOfPathAsList);
        }

        public virtual void Init()
        {
            _unit = GetComponentInParent<Unit>();

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}