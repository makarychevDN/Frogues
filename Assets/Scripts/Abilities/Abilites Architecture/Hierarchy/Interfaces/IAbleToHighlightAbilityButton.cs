using UnityEngine.Events;

namespace FroguesFramework
{
    public interface IAbleToHighlightAbilityButton
    {
        public UnityEvent<bool> GetHighlightEvent();
    }
}