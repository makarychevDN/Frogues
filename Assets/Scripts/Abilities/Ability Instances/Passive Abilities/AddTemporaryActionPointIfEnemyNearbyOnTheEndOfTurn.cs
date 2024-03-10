using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AddTemporaryActionPointIfEnemyNearbyOnTheEndOfTurn : PassiveAbility, IAbleToReturnSingleValue, IRoundTickable, IAbleToHighlightAbilityButton
    {
        [SerializeField] private int temporaryActionPointsValue;
        private UnityEvent<bool> highlightEvent = new();
        private bool lastTurnEndedNearbyEnemy;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(HashValue);
            EntryPoint.Instance.OnSomeoneMoved.AddListener(TryToHighlightButton);
            EntryPoint.Instance.OnSomeoneDied.AddListener(TryToHighlightButton);
        }

        private void HashValue()
        {
            lastTurnEndedNearbyEnemy = AnyEnemyNearby;
        }

        private void TryToHighlightButton() => highlightEvent.Invoke(AnyEnemyNearby && EntryPoint.Instance.UnitsQueue.IsUnitCurrent(_owner));

        private bool AnyEnemyNearby => _owner.CurrentCell.CellNeighbours.GetAllNeighbors().Any(cell => (!cell.IsEmpty && cell.Content.IsEnemy));

        public override void UnInit()
        {
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(HashValue);
            EntryPoint.Instance.OnSomeoneMoved.RemoveListener(TryToHighlightButton);
            EntryPoint.Instance.OnSomeoneDied.RemoveListener(TryToHighlightButton);
            base.UnInit();
        }

        public int GetValue() => temporaryActionPointsValue;

        public void TickAfterEnemiesTurn()
        {
            if (_owner == null || !_owner.AbilitiesManager.Abilities.Contains(this) || _owner.IsEnemy)
                return;

            if (lastTurnEndedNearbyEnemy)
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsValue);

            lastTurnEndedNearbyEnemy = false;
            highlightEvent.Invoke(AnyEnemyNearby);
        }

        public void TickAfterPlayerTurn()
        {
            if (_owner == null || !_owner.AbilitiesManager.Abilities.Contains(this) || !_owner.IsEnemy)
                return;

            if (lastTurnEndedNearbyEnemy)
                _owner.ActionPoints.IncreaseTemporaryPoints(temporaryActionPointsValue);

            lastTurnEndedNearbyEnemy = false;
            highlightEvent.Invoke(AnyEnemyNearby);
        }

        public UnityEvent<bool> GetHighlightEvent() => highlightEvent;
    }
}