using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AddArmorOnEndTurnIfThereIsAnyActionPoint : PassiveAbility, IAbleToReturnSingleValue, IAbleToHighlightAbilityButton
    {
        [SerializeField] private int AromorValue;
        private UnityEvent<bool> highlightEvent = new();

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TryToIncreaseArmor);
            _owner.ActionPoints.OnPointsIncreased.AddListener(TryToHighlightButton);
            _owner.ActionPoints.OnPointsEnded.AddListener(TryToHighlightButton);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TryToIncreaseArmor);
            _owner.ActionPoints.OnPointsIncreased.RemoveListener(TryToHighlightButton);
            _owner.ActionPoints.OnPointsEnded.RemoveListener(TryToHighlightButton);

        }

        public int GetValue() => AromorValue;

        private void TryToIncreaseArmor()
        {
            if(_owner.ActionPoints.CurrentPoints > 0)
            {
                _owner.Health.IncreasePermanentBlock(AromorValue);
            }
        }

        private void TryToHighlightButton() => highlightEvent.Invoke(_owner.ActionPoints.CurrentPoints > 0);

        public UnityEvent<bool> GetHighlightEvent() => highlightEvent;
    }
}