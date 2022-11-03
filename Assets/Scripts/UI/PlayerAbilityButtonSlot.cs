using UnityEngine;

namespace FroguesFramework
{
    public class PlayerAbilityButtonSlot : MonoBehaviour
    {
        [SerializeField] private AbilityButton abilityButton;

        public AbilityButton Content
        {
            set
            {
                if (!(abilityButton == null || value == null) && value.slot != null)
                    value.slot.Content = abilityButton;

                abilityButton = value;

                if (abilityButton == null)
                    return;

                abilityButton.slot = this;
                abilityButton.transform.parent = transform;
                abilityButton.transform.position = transform.position;
            }

            get => abilityButton;
        }

        public void HotkeyPressed()
        {
            abilityButton?.PickAbility();
        }
    }
}