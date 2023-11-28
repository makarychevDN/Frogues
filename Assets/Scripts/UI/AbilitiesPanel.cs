using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private AbilitiesManager abilitiesManager;
        [SerializeField] private AbilityButton abilityButtonPrefab;
        [SerializeField] private AbilityButtonSlot topAbilityCellPrefab;
        [SerializeField] private GameObject topPanel;
        [SerializeField] private List<Transform> activeAbilitySlots;
        [SerializeField] private List<Transform> passiveAbilitySlots;

        [Header("Panel Size Controller Setup")]
        [SerializeField] private RectTransform abilityButtonsSlotsPanel;
        [SerializeField] private int sizeDelta = 39;
        [SerializeField] private int slotsInTheRowQuantity = 12;

        public AbilitiesManager AbilitiesManager => abilitiesManager;
        public List<Transform> ActiveAbilitySlots => activeAbilitySlots;
        public List<Transform> PassiveAbilitySlots => passiveAbilitySlots;

        public void Init()
        {
            abilitiesManager.AbilityHasBeenAdded.AddListener(AddAbilityButton);
            abilitiesManager.AbilityHasBeenRemoved.AddListener(RemoveAbilityButton);
        }

        private void AddAbilityButton(BaseAbility ability)
        {
            var abilityAsAbleToDrawAbilityButton = ability as IAbleToDrawAbilityButton;
            
            if (abilityAsAbleToDrawAbilityButton == null || abilityAsAbleToDrawAbilityButton.IsIgnoringDrawingFunctionality())
                return;

            var abilityButton = Instantiate(abilityButtonPrefab, transform, true);
            abilityButton.Init(this, ability);
        }

        private void RemoveAbilityButton(BaseAbility ability)
        {
            foreach (var abilitySlot in activeAbilitySlots)
            {
                var button = abilitySlot.GetComponentInChildren<AbilityButton>();
                
                if(button == null || button.Ability != ability)
                    continue;
                
                DestroyImmediate(button.gameObject);
                return;
            }

            foreach (var abilitySlot in passiveAbilitySlots)
            {
                var button = abilitySlot.GetComponentInChildren<AbilityButton>();

                if (button == null || button.Ability != ability)
                    continue;

                PassiveAbilitySlots.Remove(abilitySlot);
                DestroyImmediate(abilitySlot.gameObject);
                return;
            }
        }

        public Transform FirstEmptySlot()
        {
            return activeAbilitySlots.First(slot => slot.childCount == 0);
        }

        public Transform LastEmptySlot()
        {
            return activeAbilitySlots.Last(slot => slot.childCount == 0);
        }

        public Transform AddTopAbilitySlot()
        {
            var singleCell = Instantiate(topAbilityCellPrefab, topPanel.transform);
            //singleCell.transform.localScale = Vector3.one * 2;
            passiveAbilitySlots.Add(singleCell.transform);

            return singleCell.transform;
        }

        public int SlotsInTheRowQuantity
        {
            get => slotsInTheRowQuantity;

            set
            {
                slotsInTheRowQuantity = value;
                abilityButtonsSlotsPanel.sizeDelta = new Vector2(CalculateWidth(), abilityButtonsSlotsPanel.sizeDelta.y);
            }
        }

        public void IncreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity += 1;

        public void DecreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity -= 1;

        private int CalculateWidth() => sizeDelta * slotsInTheRowQuantity;
    }
}