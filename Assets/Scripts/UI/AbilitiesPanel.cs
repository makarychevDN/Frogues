using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private AbilitiesManager abilitiesManager;
        [SerializeField] private AbilityButton abilityButtonPrefab;

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
            abilityButton.SetAbility(ability, abilityAsAbleToDrawAbilityButton);
        }
    }
}