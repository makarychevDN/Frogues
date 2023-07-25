using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilitiesStore : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private GameObject storeParentObject;
        [Header("Prefabs")]
        [SerializeField] private AbilityButton abilityButtonPrefab;
        [SerializeField] private Button selectButtonPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroupPrefab;
        [Header("Abilities")]
        [SerializeField] private List<BaseAbility> abilities;

        private List<Button> selectionButtons = new List<Button>();

        private void Start()
        {
            closeButton.onClick.AddListener(CloseAbilitiesStore);

            for (int i = 0; i < abilities.Count; i++)
            {
                var verticalLayoutGroup = Instantiate(verticalLayoutGroupPrefab, buttonsParent);

                var abilityButton = Instantiate(abilityButtonPrefab);
                abilityButton.Init(abilities[i]);
                abilityButton.transform.parent = verticalLayoutGroup.transform;
                abilityButton.transform.localScale = Vector3.one;
                abilityButton.GetComponent<Button>().interactable = false; 

                var selectionButton = Instantiate(selectButtonPrefab, verticalLayoutGroup.transform);
                var currentAbility = abilities[i];
                selectionButtons.Add(selectionButton);
                selectionButton.onClick.AddListener(() => currentAbility.Init(EntryPoint.Instance.MetaPlayer));
                selectionButton.onClick.AddListener(CloseAbilitiesStore);
                selectionButton.onClick.AddListener(() => selectionButtons.ForEach(button => button.interactable = false));
            }
        }

        public void OpenStore()
        {
            storeParentObject.SetActive(true);
        }

        private void CloseAbilitiesStore()
        {
            storeParentObject.SetActive(false);
        }
    }
}