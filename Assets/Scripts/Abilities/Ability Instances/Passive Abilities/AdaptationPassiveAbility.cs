using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AdaptationPassiveAbility : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int additionalTemporaryPoints;
        [SerializeField] private int additionalDistance;

        public int AdditionalDistance { get => additionalDistance; set => additionalDistance = value; }

        public int GetValue() => additionalTemporaryPoints;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEndOnCell.AddListener(TryToAbsorbBloodPuddle);
        }

        private void TryToAbsorbBloodPuddle(Cell cell)
        {
            List<Unit> surfaces = new();
            List<Cell> affectedCells = _owner.CurrentRoom.PathFinder.GetCellsAreaForAOE(cell, additionalDistance, true, false);
            affectedCells.Add(cell);

            foreach (Cell affectedCell in affectedCells)
            {
                foreach (Unit surface in affectedCell.Surfaces)
                {
                    if (surface is BloodPuddle)
                    {
                        surfaces.Add(surface);
                    }
                }
            }

            foreach(var surface in surfaces)
            {
                (surface as BloodPuddle).Absorb();
                _owner.ActionPoints.PickupTemporaryPoints(additionalTemporaryPoints);
            }
        }
    }
}