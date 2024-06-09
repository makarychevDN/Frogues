using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class MoveToRandomNeigborCellAI : MonoBehaviour, IAbleToAct
    {
        private Unit _owner;

        public void Act()
        {
            _owner.MovementAbility.CalculateUsingArea();

            var possibleToMovementCells = _owner.CurrentRoom.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, 1, false, false).EmptyCellsOnly();
            if (possibleToMovementCells == null || possibleToMovementCells.Count == 0 || !_owner.MovementAbility.IsResoursePointsEnough())
            {
                _owner.AbleToSkipTurn.AutoSkip();
                return;
            }

            _owner.MovementAbility.UseOnCells(new List<Cell> { possibleToMovementCells.GetRandomElement() });
        }

        public void Init(Unit owner)
        {
            _owner = owner;
        }
    }
}