using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class PushNerbyEnemiesThenHpRedusedTooMuch : PassiveAbility
    {
        [SerializeField] private List<StatEffect> effects;
        [SerializeField] private float lerpValue = 0.4f;
        private int _hashedHp;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.OnApplyUnblockedDamage.AddListener(TryToExecute);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Health.OnApplyUnblockedDamage.RemoveListener(TryToExecute);
        }

        private void TryToExecute()
        {
            float lerpedHpValue = _owner.Health.MaxHp * lerpValue;

            if (_hashedHp > lerpedHpValue && _owner.Health.CurrentHp < lerpedHpValue)
            {
                Execute();
            }

            _hashedHp = _owner.Health.CurrentHp;
        }

        protected void Execute()
        {
            List<Unit> targets = _owner.CurrentCell.CellNeighbours.GetAllNeighbors().ContentFromEachCellWioutNulls();

            foreach (Unit target in targets)
            {
                effects.ForEach(effect => target.Stats.AddStatEffect(effect));
                var hexDir = _owner.CurrentCell.CellNeighbours.GetHexDirByNeighbor(target.CurrentCell);
                var targetCell = target.CurrentCell.CellNeighbours.GetNeighborByHexDir(hexDir);
                target.Movable.Move(targetCell, 10, 0.4f, true, false);
            }
        }
    }
}