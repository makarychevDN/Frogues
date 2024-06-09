using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class MoveToClosestUnitAI : MonoBehaviour, IAbleToAct
    {
        private Unit _owner;

        public void Act()
        {
            _owner.MovementAbility.CalculateUsingArea();
            List<Unit> units = _owner.CurrentRoom.TakeAllUnits();
            units.Remove( _owner );

            if (!_owner.MovementAbility.IsResoursePointsEnough() || units == null || units.Count == 0)
            {
                SkipTurn();
                return;
            }

            int closestDistnace = units.Min(unit => unit.CurrentCell.DistanceToCell(_owner.CurrentCell, _owner.CurrentRoom));

            if (closestDistnace <= 0)
            {
                SkipTurn();
                return;
            }

            List<Unit> closestUnits = units.Where(unit => unit.CurrentCell.DistanceToCell(_owner.CurrentCell, _owner.CurrentRoom) <= closestDistnace).ToList();
            List<Cell> path = _owner.CurrentRoom.PathFinder.FindWayExcludeLastCell(_owner.CurrentCell, closestUnits.GetRandomElement().CurrentCell, false, false, true);

            if (path == null || path.Count == 0)
            {
                SkipTurn();
                return;
            }

            _owner.MovementAbility.UseOnCells(new List<Cell> { path[0] });
        }

        private void SkipTurn() => _owner.AbleToSkipTurn.AutoSkip();

        public void Init(Unit owner)
        {
            _owner = owner;
        }
    }
}