using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AddTemporaryActionPointIfDidntMoveLastTurnPassiveAbility : PassiveAbility, IRoundTickable, IAbleToHighlightAbilityButton, IAbleToReturnSingleValue
    {
        [SerializeField] private int temporaryActionPointsQuantity = 1;
        private UnityEvent<bool> onValueChanged = new();
        private bool unitWasntMoving;

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
            if (_owner == null || !_owner.AbilitiesManager.Abilities.Contains(this) || _owner.IsEnemy)
                return;

            if (unitWasntMoving)
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsQuantity);

            TurnOnBonus();
        }

        public void TickAfterPlayerTurn()
        {
            if (_owner == null || !_owner.AbilitiesManager.Abilities.Contains(this) || !_owner.IsEnemy)
                return;

            if (unitWasntMoving)
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsQuantity);

            TurnOnBonus();
        }

        private void TurnOffBonus()
        {
            unitWasntMoving = false;
            onValueChanged.Invoke(unitWasntMoving);
        }

        private void TurnOnBonus()
        {
            unitWasntMoving = true;
            onValueChanged.Invoke(unitWasntMoving);
        }

        public UnityEvent<bool> GetHighlightEvent() => onValueChanged;

        public int GetValue() => temporaryActionPointsQuantity;
    }
}