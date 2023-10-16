using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public static class UnitTaker
    {
        public static Unit GetRandomClosestUnitToUnitInRadius(this Unit unit, int radius, List<Unit> blackList = null)
        {
            List<Cell> cells = new();
            List<Unit> units = new();

            for(int i = 0; i < radius + 1; i++)
            {
                cells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(unit.CurrentCell, i, true, false);

                if (cells.Where(cell => !cell.IsEmpty && !blackList.Contains(cell.Content)).Count() != 0)
                    break;
            }

            foreach (Cell cell in cells.Where(cell => !cell.IsEmpty))
            {
                units.Add(cell.Content);
            }

            if(blackList != null)
            {
                foreach(var bannedUnit in blackList)
                {
                    if (units.Contains(bannedUnit))
                    {
                        units.Remove(bannedUnit);
                    }
                }
            }

            return units.GetRandomElement();
        }
    }
}