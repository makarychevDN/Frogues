using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AddTemporaryActionPointIfEnemyNearbyOnTheEndOfTurn : PassiveAbility
    {
        [SerializeField] private int temporaryActionPointsValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TryToAddTemporaryPoint);
        }

        private void TryToAddTemporaryPoint()
        {
            if(_owner.CurrentCell.CellNeighbours.GetAllNeighbors().Any(cell => cell.Content.IsEnemy))
            {
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsValue);
            }
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TryToAddTemporaryPoint);
        }
    }
}