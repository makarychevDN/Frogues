using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class IncreasePickUpBloodDistance : PassiveAbility
    {
        [SerializeField] private int distance;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEndOnCell.AddListener(TryToStepOnBloodOnTheDistance);
        }

        private void TryToStepOnBloodOnTheDistance(Cell unitsCell)
        {
            List<Cell> neighborCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(unitsCell, distance, true, false);
            foreach (Cell neighborCell in neighborCells)
            {
                List<Unit> surfaces = new();
                foreach (Unit surface in neighborCell.Surfaces)
                {
                    try
                    {
                        if(surface.AbilitiesManager.Abilities.Any(ability => ability is IncreaseBloodPointsOnStepOnSurface))
                        {
                            surfaces.Add(surface);
                        }
                    }
                    catch
                    {
                        print(surface);
                        print(surface.AbilitiesManager);
                        print(surface.AbilitiesManager.Abilities);
                        print(surface.AbilitiesManager.Abilities.Any(ability => ability is IncreaseBloodPointsOnStepOnSurface));
                    }
                }
                surfaces.ForEach(surface => surface.OnStepOnThisUnitByUnit.Invoke(_owner));
            }
        }
    }
}