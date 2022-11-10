using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private AbilitiesManager abilitiesManager;
        [SerializeField] private AbilityButton abilityButtonPrefab;
        [SerializeField] private List<Transform> abilitySlots;

        public AbilitiesManager AbilitiesManager => abilitiesManager;

        private void Awake()
        {
            abilitiesManager.AbilityHasBeenAdded.AddListener(AddAbilityButton);
        }

        private void AddAbilityButton(IAbility ability)
        {
            var abilityAsAbleToDrawAbilityButton = ability as IAbleToDrawAbilityButton;
            
            if (abilityAsAbleToDrawAbilityButton == null)
                return;

            var abilityButton = Instantiate(abilityButtonPrefab, transform, true);
            abilityButton.Init(this, ability, abilityAsAbleToDrawAbilityButton);
        }

        public Transform FirstEmptySlot()
        {
            return abilitySlots.First(slot => slot.childCount == 0);
        }
    }
}