using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class RollAbility : JumpOnCellAbility, IAbleToApplyBlock
    {
        [SerializeField] private int blockValue;

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            base.UseOnCells(cells);
            _owner.Movable.OnMovementEnd.AddListener(IncreaseTemporaryBlock);
        }

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, CellsTaker.ObstacleMode.onlyBigUnitsAreObstacles, false, true).ToList()
                .Where(cell => cell.DistanceToCell(_owner.CurrentCell) == range).ToList();
        }

        private void IncreaseTemporaryBlock()
        {
            _owner.Health.IncreaseBlock(CalculateBlock());
            _owner.Movable.OnMovementEnd.RemoveListener(IncreaseTemporaryBlock);
        }

        private int CalculateBlock() => Extensions.CalculateBlockWithGameRules(blockValue, _owner.Stats);

        public int GetDefaultBlockValue() => blockValue;

        int IAbleToApplyBlock.CalculateBlock() => CalculateBlock();
    }
}