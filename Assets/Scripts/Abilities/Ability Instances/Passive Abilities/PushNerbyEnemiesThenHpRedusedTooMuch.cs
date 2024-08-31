using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class PushNerbyEnemiesThenHpRedusedTooMuch : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int requredPercentageOfHPToExecute = 40;
        private int _hashedHp;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.OnDamageAppledByHealth.AddListener(TryToExecute);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Health.OnDamageAppledByHealth.RemoveListener(TryToExecute);
        }

        private void TryToExecute()
        {
            float lerpedHpValue = _owner.Health.MaxHp * ConvertToLerpValue(requredPercentageOfHPToExecute);

            if (_hashedHp > lerpedHpValue && _owner.Health.CurrentHp < lerpedHpValue)
            {
                Execute();
            }

            _hashedHp = _owner.Health.CurrentHp;
        }

        private float ConvertToLerpValue(int value) => value * 0.01f;

        protected void Execute()
        {
            List<Unit> targets = _owner.CurrentCell.CellNeighbours.GetAllNeighbors().ContentFromEachCellWioutNulls();

            foreach (Unit target in targets)
            {
                var hexDir = _owner.CurrentCell.CellNeighbours.GetHexDirByNeighbor(target.CurrentCell);
                var targetCell = target.CurrentCell.CellNeighbours.GetNeighborByHexDir(hexDir);
                target.Movable.Move(targetCell, 10, 0.4f, true, false);
            }
        }

        public int GetValue() => requredPercentageOfHPToExecute;
    }
}