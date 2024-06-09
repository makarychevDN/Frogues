using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UseAreaTargetAbilityOrSkipTurnAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private AreaTargetAbility areaTargetAbility;
        private Unit _owner;
        private Unit _target;

        public void Act()
        {
            _target = _owner.CurrentRoom.PlayableCharacters[0];
            areaTargetAbility.CalculateUsingArea();
            var targetCellToList = new List<Cell> { _target.CurrentCell };
            areaTargetAbility.PrepareToUsing(targetCellToList);
            var selectedCells = areaTargetAbility.SelectCells(targetCellToList);


            if (areaTargetAbility.PossibleToUseOnCells(selectedCells))
            {
                areaTargetAbility.UseOnCells(selectedCells);
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