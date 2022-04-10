using System.Collections.Generic;
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
    }
}