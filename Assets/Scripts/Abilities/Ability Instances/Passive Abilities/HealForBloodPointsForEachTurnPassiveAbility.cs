using UnityEngine;

namespace FroguesFramework
{
    public class HealForBloodPointsForEachTurnPassiveAbility : PassiveAbility, IRoundTickable, IAbleToCost, IAbleToReturnSingleValue
    {
        [SerializeField] private int cost;
        [SerializeField] private int healingValue;
        private bool inited;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            inited = true;
        }

        public override void UnInit()
        {
            inited = true;
            base.UnInit();
        }

        public void TickAfterEnemiesTurn()
        {
            if (!inited)
                return;

            if (_owner.IsEnemy)
            {
                TryToExecute();
            }
        }

        public void TickAfterPlayerTurn()
        {
            if (!inited)
                return;

            if (!_owner.IsEnemy)
            {
                TryToExecute();
            }
        }

        private void TryToExecute()
        {
            if (EntryPoint.Instance.CurrentRoomIsPeaceful || EntryPoint.Instance.ExitActivated)
                return;

            if (IsResoursePointsEnough() && !_owner.Health.Full)
            {
                SpendResourcePoints();
                _owner.Health.TakeHealing(healingValue);
            }
        }

        public int GetActionPointsCost() => 0;

        public int GetBloodPointsCost() => cost;

        public int GetHealthCost() => -healingValue;

        public bool IsResoursePointsEnough() => _owner.BloodPoints.IsPointsEnough(cost);

        public void SpendResourcePoints() => _owner.BloodPoints.SpendPoints(cost);

        public int GetValue() => healingValue;
    }
}