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

        public List<Transform> AbilitySlots => abilitySlots;

        public void Init()
        {
            abilitiesManager.AbilityHasBeenAdded.AddListener(AddAbilityButton);
            abilitiesManager.AbilityHasBeenRemoved.AddListener(RemoveAbilityButton);
        }

        private void AddAbilityButton(BaseAbility ability)
        {
            var abilityAsAbleToDrawAbilityButton = ability as IAbleToDrawAbilityButton;
            
            if (abilityAsAbleToDrawAbilityButton == null)
                return;

            var abilityButton = Instantiate(abilityButtonPrefab, transform, true);
            abilityButton.Init(this, ability, abilityAsAbleToDrawAbilityButton);
        }

        private void RemoveAbilityButton(BaseAbility ability)
        {
            foreach (var abilitySlot in abilitySlots)
            {
                var button = abilitySlot.GetComponentInChildren<AbilityButton>();
                
                if(button == null || button.Ability != ability)
                    continue;
                
                DestroyImmediate(button.gameObject);
                return;
            }
        }

        public Transform FirstEmptySlot()
        {
            return abilitySlots.First(slot => slot.childCount == 0);
        }

        public Transform LastEmptySlot()
        {
            return abilitySlots.Last(slot => slot.childCount == 0);
        }
    }
}