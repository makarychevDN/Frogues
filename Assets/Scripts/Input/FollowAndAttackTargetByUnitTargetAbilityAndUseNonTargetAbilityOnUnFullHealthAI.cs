using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAndUseNonTargetAbilityOnUnFullHealthAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility unitTargetAbilty;
        [SerializeField] private NonTargetAbility splitOnSmallSlimes;
        private Unit _unit;

        public void Act()
        {
            if (!_unit.Health.Full)
            {
                splitOnSmallSlimes.Use();
                return;
            }


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

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}