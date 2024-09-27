using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AddArmorOnEndTurnForExtraActionPoints : PassiveAbility, IAbleToReturnSingleValue, IAbleToHighlightAbilityButton, IAbleToApplyArmor
    {
        [SerializeField] private int AromorValue;
        private UnityEvent<bool> highlightEvent = new();

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.ActionPoints.OnTemporaryPointsResetedOnValue.AddListener(TryToIncreaseArmor);
            _owner.ActionPoints.OnTemporaryPointsIncreased.AddListener(TryToHighlightButton);
            _owner.ActionPoints.OnPointsSpent.AddListener(TryToHighlightButton);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.ActionPoints.OnTemporaryPointsResetedOnValue.RemoveListener(TryToIncreaseArmor);
            _owner.ActionPoints.OnAnyPointsIncreased.RemoveListener(TryToHighlightButton);
            _owner.ActionPoints.OnPointsSpent.RemoveListener(TryToHighlightButton);

        }

        public int GetValue() => AromorValue;

        private void TryToIncreaseArmor(int temporaryPoints)
        {
            if(_owner.ActionPoints.TemporaryPoints > 0)
            {
                _owner.Health.IncreaseArmor(temporaryPoints * AromorValue);
            }
        }

        private void TryToHighlightButton()
        {
            highlightEvent.Invoke(_owner.ActionPoints.TemporaryPoints > 0);
        }

        public UnityEvent<bool> GetHighlightEvent() => highlightEvent;

        public int GetDefaultArmorValue() => AromorValue;

        public int CalculateArmor() => AromorValue;
    }
}