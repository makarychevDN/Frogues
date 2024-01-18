using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class GetTemporaryActionPointsWhenYouRunFromEnemies : PassiveAbility, IRoundTickable, IAbleToReturnSingleValue
    {
        [SerializeField] private int additionalTemporaryActionPoints;
        private bool ownerEscapedOnThisTurnAlready;
        private Cell lastCell;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementStartFromCell.AddListener(HashCell);
            _owner.Movable.OnMovementEndOnCell.AddListener(TryToEnableBonus);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Movable.OnMovementStartFromCell.RemoveListener(HashCell);
            _owner.Movable.OnMovementEndOnCell.RemoveListener(TryToEnableBonus);
        }

        private bool AnyEnemyNearby(Cell cell) => cell.CellNeighbours.GetAllNeighbors().Any(cell => (!cell.IsEmpty && cell.Content.IsEnemy));

        private void HashCell(Cell cell) => lastCell = cell;

        private void TryToEnableBonus(Cell reachedCell)
        {
            if (ownerEscapedOnThisTurnAlready)
                return;

            if(AnyEnemyNearby(lastCell) && !AnyEnemyNearby(reachedCell))
            {
                _owner.ActionPoints.IncreaseTemporaryPoints(additionalTemporaryActionPoints);
                ownerEscapedOnThisTurnAlready = true;
            }
        }

        public void TickAfterEnemiesTurn()
        {
            if (_owner.IsEnemy)
                return;

            ownerEscapedOnThisTurnAlready = false;
        }

        public void TickAfterPlayerTurn()
        {
            if (!_owner.IsEnemy)
                return;

            ownerEscapedOnThisTurnAlready = false;
        }

        public int GetValue() => additionalTemporaryActionPoints;
    }
}