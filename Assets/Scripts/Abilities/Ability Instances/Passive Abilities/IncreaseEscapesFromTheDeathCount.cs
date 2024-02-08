using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class IncreaseEscapesFromTheDeathCount : PassiveAbility, IAbleToHighlightAbilityButton, IAbleToHaveCount
    {
        [SerializeField] private int additionalEscapesFromDeath;
        [SerializeField] private RuntimeAnimatorController animatorAfterTailDestroyed;
        private bool shouldHiglightButton;
        private UnityEvent<bool> highlightButtonEvent = new();

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseEscapesFromDeathCount(additionalEscapesFromDeath);
            _owner.Health.OnHpEnded.AddListener(UpdateStatus);
            shouldHiglightButton = true;
            highlightButtonEvent.Invoke(shouldHiglightButton);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Health.IncreaseEscapesFromDeathCount(-additionalEscapesFromDeath);
            _owner.Health.OnHpEnded.RemoveListener(UpdateStatus);
            shouldHiglightButton = false;
            highlightButtonEvent.Invoke(shouldHiglightButton);
        }

        public void UpdateStatus()
        {
            shouldHiglightButton = false;
            highlightButtonEvent.Invoke(shouldHiglightButton);

            if(animatorAfterTailDestroyed != null)
            {
                _owner.Animator.runtimeAnimatorController = animatorAfterTailDestroyed;
            }
        }

        public UnityEvent<bool> GetHighlightEvent() => highlightButtonEvent;

        public int GetCount() => additionalEscapesFromDeath;
    }
}
