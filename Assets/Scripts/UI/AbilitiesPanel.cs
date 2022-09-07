using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesPanel : MonoBehaviour
    {
        public static AbilitiesPanel Instance;
        public List<PlayerAbilityButtonSlot> abilitySlots;

        private void Start()
        {
            Instance = this;
        }

        public void AddAbilityButton(AbilityButton abilityButton)
        {
            abilitySlots.FirstOrDefault(slot => slot.Content == null).Content = abilityButton;
        }
    }
}