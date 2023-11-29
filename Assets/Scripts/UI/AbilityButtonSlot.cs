using UnityEngine;

namespace FroguesFramework
{
    public class AbilityButtonSlot : MonoBehaviour
    {
        [SerializeField] private AbilityButton abilityButton;
        [SerializeField] private Transform buttonHolder;
        [SerializeField] private HotKey hotkey;

        public void AddButton(AbilityButton button)
        {
            abilityButton = button;
            button.transform.parent = buttonHolder;
            button.transform.localPosition = Vector3.zero;
            button.transform.localScale = Vector3.one;
        }

        public void Clear()
        {
            abilityButton = null;
        }

        public bool Empty => abilityButton == null;

        public AbilityButton AbilityButton => abilityButton;

        public HotKey HotKey => hotkey;
    }
}