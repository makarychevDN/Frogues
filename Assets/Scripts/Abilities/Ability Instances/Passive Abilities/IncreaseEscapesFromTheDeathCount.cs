using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class IncreaseEscapesFromTheDeathCount : PassiveAbility, IAbleToHighlightAbilityButton
    {
        [SerializeField] private int additionalEscapesFromDeath;
        private bool shouldHiglightButton;
        private UnityEvent<bool> highlightButtonEvent = new();

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseEscapesFromDeathCount(additionalEscapesFromDeath);
            _owner.Health.OnHpEnded.AddListener(UpdatedStatus);
            shouldHiglightButton = true;
            highlightButtonEvent.Invoke(shouldHiglightButton);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Health.IncreaseEscapesFromDeathCount(-additionalEscapesFromDeath);
            _owner.Health.OnHpEnded.RemoveListener(UpdatedStatus);
            shouldHiglightButton = false;
            highlightButtonEvent.Invoke(shouldHiglightButton);
        }

        public void UpdatedStatus()
        {
            shouldHiglightButton = false;
            highlightButtonEvent.Invoke(shouldHiglightButton);
        }

        public UnityEvent<bool> GetHighlightEvent() => highlightButtonEvent;
    }
}
