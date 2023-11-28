using UnityEngine;

namespace FroguesFramework
{
    public class AbilityButtonSlot : MonoBehaviour
    {
        [SerializeField] private AbilityButton abilityButton;

        public void AddButton(AbilityButton button)
        {
            abilityButton = button;
            button.transform.parent = transform;
            button.transform.localPosition = Vector3.zero;
        }

        public void Clear()
        {
            abilityButton = null;
        }

        public bool Empty => abilityButton == null;

        public AbilityButton AbilityButton => abilityButton;
    }
}