using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveUnitOnDeathPassiveAbility : IncreaseHpAndBloodAndAddStatEffects, IAbleToReturnRange
    {
        [SerializeField] private Unit unitPrefab;
        [SerializeField] private int radius;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.OnHpEnded.AddListener(TryToSpawnAndMoveUnit);
        }

        public override void UnInit()
        {
            _owner.Health.OnHpEnded.RemoveListener(TryToSpawnAndMoveUnit);
            base.UnInit();
        }

        private void TryToSpawnAndMoveUnit()
        {
            var cells = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius).EmptyCellsOnly();

            if (cells.Count == 0)
                return;

            EntryPoint.Instance.SpawnUnit(unitPrefab, _owner, cells.GetRandomElement());
        }

        public int ReturnRange() => radius;
    }
}