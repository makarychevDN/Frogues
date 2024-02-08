using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SplitSlimeOnSmallSlimes : NonTargetAbility, IAbleToReturnRange, IAbleToReturnSingleValue, IAbleToReturnSecondSingleValue
    {
        [SerializeField] private int radius;
        [SerializeField] private int samllSlimesQuantity;
        [SerializeField] private List<UnitAndCount> unitAndCounts;

        public int GetValue() => unitAndCounts[0].count;

        public int GetSecondValue() => unitAndCounts[1].count;

        public int ReturnRange() => radius;

        public override void Use()
        {
            foreach (var unitAndCount in unitAndCounts)
            {
                for (int i = 0; i < unitAndCount.count; i++)
                {
                    var emptyCells = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius).EmptyCellsOnly();

                    if (emptyCells == null || emptyCells.Count == 0)
                        break;

                    EntryPoint.Instance.SpawnUnit(unitAndCount.unit, _owner, emptyCells.GetRandomElement());
                }
            }

            _owner.AbleToDie.Die();
        }

        [Serializable]
        public struct UnitAndCount
        {
            public Unit unit;
            public int count;
        }
    }
}