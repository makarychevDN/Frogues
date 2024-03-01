using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class MoveToClosestUnitAI : MonoBehaviour, IAbleToAct
    {
        private Unit _unit;

        public void Act()
        {
            _unit.MovementAbility.CalculateUsingArea();
            List<Unit> units = CellsTaker.TakeAllUnits();
            units.Remove( _unit );

            if (!_unit.MovementAbility.IsResoursePointsEnough() || units == null || units.Count == 0)
            {
                print("1");
                SkipTurn();
                return;
            }

            int closestDistnace = units.Min(unit => unit.CurrentCell.DistanceToCell(_unit.CurrentCell));

            if (closestDistnace <= 0)
            {
                print("2");
                SkipTurn();
                return;
            }

            List<Unit> closestUnits = units.Where(unit => unit.CurrentCell.DistanceToCell(_unit.CurrentCell) <= closestDistnace).ToList();
            List<Cell> path = EntryPoint.Instance.PathFinder.FindWayExcludeLastCell(_unit.CurrentCell, closestUnits.GetRandomElement().CurrentCell, false, false, true);

            if (path == null || path.Count == 0)
            {
                print("3");
                SkipTurn();
                return;
            }

            _unit.MovementAbility.UseOnCells(new List<Cell> { path[0] });
        }

        private void SkipTurn() => _unit.AbleToSkipTurn.AutoSkip();

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
        }
    }
}