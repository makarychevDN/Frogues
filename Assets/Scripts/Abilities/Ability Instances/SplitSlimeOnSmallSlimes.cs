using UnityEngine;

namespace FroguesFramework
{
    public class SplitSlimeOnSmallSlimes : NonTargetAbility, IAbleToReturnRange, IAbleToHaveCount
    {
        [SerializeField] private int radius;
        [SerializeField] private int samllSlimesQuantity;
        [SerializeField] private Unit smallSlimePrefab;

        public int GetCount() => samllSlimesQuantity;

        public int ReturnRange() => radius;

        public override void Use()
        {
            for (int i = 0; i < samllSlimesQuantity; i++) 
            {
                var emptyCells = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius).EmptyCellsOnly();

                if (emptyCells == null || emptyCells.Count == 0)
                    break;

                EntryPoint.Instance.SpawnUnit(smallSlimePrefab, _owner, emptyCells.GetRandomElement());
            }

            _owner.AbleToDie.Die();
        }
    }
}