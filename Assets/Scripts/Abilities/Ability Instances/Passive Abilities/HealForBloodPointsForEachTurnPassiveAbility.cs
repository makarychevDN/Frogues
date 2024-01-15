using UnityEngine;

namespace FroguesFramework
{
    public class HealForBloodPointsForEachTurnPassiveAbility : PassiveAbility, IRoundTickable
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
            if (_owner.BloodPoints.IsPointsEnough(cost) && !_owner.Health.Full)
            {
                _owner.BloodPoints.SpendPoints(cost);
                _owner.Health.TakeHealing(healingValue);
            }
        }
    }
}