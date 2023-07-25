using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilitiesStore : MonoBehaviour
    {
        [SerializeField] private List<BaseAbility> abilities;
        [SerializeField] private List<AbilityButton> abilityButtons;
        [SerializeField] private AbilityButton abilityButtonPrefab;
        [SerializeField] private Button selectButtonPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroupPrefab;
        [SerializeField] private Transform buttonsParent;

        private void Start()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                var verticalLayoutGroup = Instantiate(verticalLayoutGroupPrefab, buttonsParent);

                var abilityButton = Instantiate(abilityButtonPrefab);
                abilityButton.Init(abilities[i]);
                abilityButton.transform.parent = verticalLayoutGroup.transform;
                abilityButton.transform.localScale = Vector3.one;
                abilityButton.GetComponent<Button>().interactable = false; 
                abilityButtons.Add(abilityButton);

                var selectionButton = Instantiate(selectButtonPrefab, verticalLayoutGroup.transform);
                var currentAbility = abilities[i];
                selectionButton.onClick.AddListener(() => currentAbility.Init(EntryPoint.Instance.MetaPlayer));
            }
        }
    }
}