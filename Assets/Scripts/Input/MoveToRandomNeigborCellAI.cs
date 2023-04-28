using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class MoveToRandomNeigborCellAI : MonoBehaviour, IAbleToAct
    {
        private Unit _unit;

        public void Act()
        {
            _unit.MovementAbility.CalculateUsingArea();

            var possibleToMovementCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_unit.CurrentCell, 1, false, false);
            if (possibleToMovementCells == null || possibleToMovementCells.Count == 0 || !_unit.MovementAbility.IsActionPointsEnough())
            {
                _unit.AbleToSkipTurn.AutoSkip();
                return;
            }

            _unit.MovementAbility.UseOnCells(new List<Cell> { possibleToMovementCells.GetRandomElement() });
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
        }
    }
}