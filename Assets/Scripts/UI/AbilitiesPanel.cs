using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private AbilitiesManager abilitiesManager;
        [SerializeField] private AbilityButton abilityButtonPrefab;
        [SerializeField] private AbilityButtonSlot topAbilitySlotPrefab;
        [SerializeField] private AbilityButtonSlot bottomAbilitySlotPrefab;
        [SerializeField] private GameObject topPanel;
        [SerializeField] private List<AbilityButtonSlot> activeAbilitySlots;
        [SerializeField] private List<AbilityButtonSlot> passiveAbilitySlots;

        [Header("Panel Size Controller Setup")]
        [SerializeField] private RectTransform abilityButtonsSlotsPanel;
        [SerializeField] private Transform parentForBottomAbilitySlots;
        [SerializeField] private int sizeDelta = 39;
        [SerializeField] private int slotsInTheRowQuantity = 12;
        [SerializeField] private int minRowsQuantity = 3;
        [SerializeField] private bool showThemAll;
        [SerializeField] private int currentShowingRow = 0;
        private int currentRowsQuantity;

        public AbilitiesManager AbilitiesManager => abilitiesManager;
        public List<AbilityButtonSlot> ActiveAbilitySlots => activeAbilitySlots;
        public List<AbilityButtonSlot> PassiveAbilitySlots => passiveAbilitySlots;

        public void Init()
        {
            abilitiesManager.AbilityHasBeenAdded.AddListener(AddAbilityButton);
            abilitiesManager.AbilityHasBeenRemoved.AddListener(RemoveAbilityButton);
            currentRowsQuantity = minRowsQuantity;

            UpdateEnabledSlots();
        }

        private void UpdateEnabledSlots()
        {
            if (activeNowSlotsCount > currentRowsQuantity * slotsInTheRowQuantity)
            {
                while (activeNowSlotsCount > currentRowsQuantity * slotsInTheRowQuantity)
                {
                    activeAbilitySlots.Last(slot => slot.gameObject.activeSelf).gameObject.SetActive(false);
                }
            }

            if (activeAbilitySlots.Where(slot => slot.gameObject.activeSelf).Count() < currentRowsQuantity * slotsInTheRowQuantity)
            {
                while (activeAbilitySlots.Where(slot => slot.gameObject.activeSelf).Count() < currentRowsQuantity * slotsInTheRowQuantity)
                {
                    var currentSlot = activeAbilitySlots.FirstOrDefault(slot => !slot.gameObject.activeSelf);

                    if (currentSlot == null)
                        activeAbilitySlots.Add(currentSlot = Instantiate(bottomAbilitySlotPrefab, parentForBottomAbilitySlots));

                    currentSlot.gameObject.SetActive(true);
                }
            }

            if (showThemAll)
            {
                for (int i = 0; i < slotsInTheRowQuantity * currentRowsQuantity; i++)
                {
                    activeAbilitySlots[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < activeAbilitySlots.Count; i++)
                {
                    activeAbilitySlots[i].gameObject.SetActive(i >= currentShowingRow * slotsInTheRowQuantity && i < (currentShowingRow + 1) * slotsInTheRowQuantity);
                }
            }
        }

        public void SetShowThemAllMode(bool value)
        {
            showThemAll = value;
            UpdateEnabledSlots();
        }

        public void SwitchShowThemAllMode()
        {
            showThemAll = !showThemAll;
            UpdateEnabledSlots();
        }

        private int activeNowSlotsCount => activeAbilitySlots.Where(slot => slot.gameObject.activeSelf).Count();

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

        public AbilityButtonSlot FirstEmptySlot()
        {
            if(activeAbilitySlots.None(slot => slot.Empty))
            {
                currentRowsQuantity++;
                UpdateEnabledSlots();
            }

            return activeAbilitySlots.First(slot => slot.Empty);
        }

        public AbilityButtonSlot AddTopAbilitySlot()
        {
            var singleCell = Instantiate(topAbilitySlotPrefab, topPanel.transform);
            passiveAbilitySlots.Add(singleCell);

            return singleCell;
        }

        public int SlotsInTheRowQuantity
        {
            get => slotsInTheRowQuantity;

            set
            {
                slotsInTheRowQuantity = value;
                abilityButtonsSlotsPanel.sizeDelta = new Vector2(CalculateWidth(), abilityButtonsSlotsPanel.sizeDelta.y);

                UpdateEnabledSlots();
            }
        }

        public void IncreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity += 1;

        public void DecreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity -= 1;

        private int CalculateWidth() => sizeDelta * slotsInTheRowQuantity;
    }
}