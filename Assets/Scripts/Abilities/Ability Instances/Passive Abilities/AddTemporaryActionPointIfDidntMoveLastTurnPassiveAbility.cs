using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AddTemporaryActionPointIfDidntMoveLastTurnPassiveAbility : PassiveAbility, IRoundTickable, IAbleToHighlightAbilityButton, IAbleToReturnSingleValue
    {
        [SerializeField] private int temporaryActionPointsQuantity = 1;
        private UnityEvent<bool> onValueChanged = new();
        private bool unitWasntMove;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEnd.AddListener(TurnOffBonus);
        }

        public override void UnInit()
        {
            _owner.Movable.OnMovementEnd.RemoveListener(TurnOffBonus);
            base.UnInit();
        }

        public void TickAfterEnemiesTurn()
        {
            if (_owner.IsEnemy)
                return;

            if (unitWasntMove)
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsQuantity);

            TurnOnBonus();
        }

        public void TickAfterPlayerTurn()
        {
            if (!_owner.IsEnemy)
                return;

            if (unitWasntMove)
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsQuantity);

            TurnOnBonus();
        }

        private void TurnOffBonus()
        {
            unitWasntMove = false;
            onValueChanged.Invoke(unitWasntMove);
        }

        private void TurnOnBonus()
        {
            unitWasntMove = true;
            onValueChanged.Invoke(unitWasntMove);
        }

        public UnityEvent<bool> GetHighlightEvent() => onValueChanged;

        public int GetValue() => temporaryActionPointsQuantity;
    }
}