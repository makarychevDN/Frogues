using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesOfPlayerController : MonoBehaviour
    {
        [SerializeField] private List<AbilityAndButton> abilities = new List<AbilityAndButton>();
        
        public void AddAbility(Ability ability)
        {
            var abilityButton = Instantiate(ability.AbilityButtonPrefab);
            FindObjectOfType<AbilitiesPanel>().AddAbilityButton(abilityButton);
            abilityButton.SetAbility(ability);
            
            abilities.Add(new AbilityAndButton(ability, abilityButton));
        }

        public void RemoveAbility(Ability ability)
        {
            var structInstance = abilities.FirstOrDefault(structInstance => structInstance.ability == ability);
            abilities.Remove(structInstance);
            structInstance.abilityButton.slot.Content = null;
            Destroy(structInstance.abilityButton.gameObject);
        }

        public void RemoveAllWeaponAbilities()
        {
            while (abilities.Any(abilityAndButton => abilityAndButton.ability.ShouldRemoveOnChangeWeapon))
            {
                RemoveAbility(abilities
                    .FirstOrDefault(abilityAndButton => abilityAndButton.ability.ShouldRemoveOnChangeWeapon).ability);
            }
        }
        
        [Serializable] 
        private struct AbilityAndButton
        {
            public Ability ability;
            public AbilityButton abilityButton;

            public AbilityAndButton(Ability ability, AbilityButton abilityButton)
            {
                this.ability = ability;
                this.abilityButton = abilityButton;
            }
        }
    }
}
