using UnityEngine.Events;

namespace FroguesFramework
{
    public abstract class BattleStanceAbility : NonTargetAbility, IAbleToHighlightAbilityButton
    {
        protected bool stanceActiveNow;
        public UnityEvent<BattleStanceAbility> OnThisStanceSelected;
        public UnityEvent<bool> OnActiveNowUpdated;

        public override void Use()
        {
            if (!IsResoursePointsEnough())
                return;

            SpendResourcePoints();
            stanceActiveNow = !stanceActiveNow;
            OnActiveNowUpdated.Invoke(stanceActiveNow);
            ApplyEffect(stanceActiveNow);
        }

        public virtual void ApplyEffect(bool isActive)
        {
            if (isActive)
            {
                OnThisStanceSelected.Invoke(this);
            }
        }

        public UnityEvent<bool> GetHighlightEvent() => OnActiveNowUpdated;
    }
}