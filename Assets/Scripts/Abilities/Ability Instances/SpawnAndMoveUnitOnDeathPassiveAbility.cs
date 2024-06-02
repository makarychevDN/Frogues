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
            _owner.AbleToDie.OnDeath.AddListener(TryToSpawnAndMoveUnit);
        }

        public override void UnInit()
        {
            _owner.AbleToDie.OnDeath.RemoveListener(TryToSpawnAndMoveUnit);
            base.UnInit();
        }

        private void TryToSpawnAndMoveUnit()
        {
            var cells = _owner.CurrentRoom.TakeCellsAreaByRange(_owner.CurrentCell, radius).EmptyCellsOnly();

            if (cells.Count == 0)
                return;

            var spawnedBug = Extensions.SpawnUnit(unitPrefab, _owner, cells.GetRandomElement(), _owner.CurrentRoom);
            spawnedBug.IsSummoned = true;
        }

        public int ReturnRange() => radius;
    }
}