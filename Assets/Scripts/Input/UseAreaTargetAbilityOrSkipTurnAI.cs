using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UseAreaTargetAbilityOrSkipTurnAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private AreaTargetAbility areaTargetAbility;
        private Unit _unit;
        private Unit _target;

        public void Act()
        {
            areaTargetAbility.CalculateUsingArea();
            var targetCellToList = new List<Cell> { _target.CurrentCell };
            var selectedCells = areaTargetAbility.SelectCells(targetCellToList);


            if (areaTargetAbility.PossibleToUseOnCells(selectedCells))
            {
                areaTargetAbility.UseOnCells(selectedCells);
                return;
            }

            _unit.AbleToSkipTurn.AutoSkip();
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _target = EntryPoint.Instance.MetaPlayer;
        }
    }
}