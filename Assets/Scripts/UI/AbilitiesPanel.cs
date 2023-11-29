using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        [SerializeField] private List<AbilityButtonSlot> bottomPanelAbilitySlots;
        [SerializeField] private List<AbilityButtonSlot> topPanelAbilitySlots;

        [Header("Panel Size Controller Setup")]
        [SerializeField] private RectTransform abilityButtonsSlotsPanel;
        [SerializeField] private Transform parentForBottomAbilitySlots;
        [SerializeField] private int sizeDelta = 39;
        [SerializeField] private int slotsInTheRowQuantity = 12;
        [SerializeField] private int minRowsQuantity = 3;
        [SerializeField] private bool showThemAll;
        [SerializeField] private int currentShowingRow = 0;
        [SerializeField] private int minSlotsInTheRowQuantity = 10;
        [SerializeField] private int maxSlotsInTheRowQuantity = 20;

        [Header("Scrolling Indicator Setup")]
        [SerializeField] private TMP_Text currentPageIndexIndicator;

        private int currentRowsQuantity;

        public AbilitiesManager AbilitiesManager => abilitiesManager;
        public List<AbilityButtonSlot> ActiveAbilitySlots => bottomPanelAbilitySlots;
        public List<AbilityButtonSlot> PassiveAbilitySlots => topPanelAbilitySlots;

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
                    var slotToDisable = bottomPanelAbilitySlots.Last(slot => slot.gameObject.activeSelf && slot.Empty);
                    
                    slotToDisable.transform.SetAsLastSibling();
                    bottomPanelAbilitySlots.Remove(slotToDisable);
                    bottomPanelAbilitySlots.Add(slotToDisable); // to place it in the end of the list

                    slotToDisable.gameObject.SetActive(false);
                }
            }

            if (bottomPanelAbilitySlots.Where(slot => slot.gameObject.activeSelf).Count() < currentRowsQuantity * slotsInTheRowQuantity)
            {
                while (bottomPanelAbilitySlots.Where(slot => slot.gameObject.activeSelf).Count() < currentRowsQuantity * slotsInTheRowQuantity)
                {
                    var currentSlot = bottomPanelAbilitySlots.FirstOrDefault(slot => !slot.gameObject.activeSelf);

                    if (currentSlot == null)
                        bottomPanelAbilitySlots.Add(currentSlot = Instantiate(bottomAbilitySlotPrefab, parentForBottomAbilitySlots));

                    currentSlot.gameObject.SetActive(true);
                }
            }

            if (showThemAll)
            {
                for (int i = 0; i < slotsInTheRowQuantity * currentRowsQuantity; i++)
                {
                    bottomPanelAbilitySlots[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < bottomPanelAbilitySlots.Count; i++)
                {
                    bottomPanelAbilitySlots[i].gameObject.SetActive(i >= currentShowingRow * slotsInTheRowQuantity && i < (currentShowingRow + 1) * slotsInTheRowQuantity);
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

        private int activeNowSlotsCount => bottomPanelAbilitySlots.Where(slot => slot.gameObject.activeSelf).Count();
        private int fullSlotsCount => bottomPanelAbilitySlots.Where(slot => !slot.Empty).Count();

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
            foreach (var abilitySlot in bottomPanelAbilitySlots)
            {
                var button = abilitySlot.GetComponentInChildren<AbilityButton>();

                if (button == null || button.Ability != ability)
                    continue;

                DestroyImmediate(button.gameObject);
                return;
            }

            foreach (var abilitySlot in topPanelAbilitySlots)
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
            if (bottomPanelAbilitySlots.None(slot => slot.Empty))
            {
                currentRowsQuantity++;
                UpdateEnabledSlots();
            }

            return bottomPanelAbilitySlots.First(slot => slot.Empty);
        }

        public AbilityButtonSlot AddTopAbilitySlot()
        {
            var singleCell = Instantiate(topAbilitySlotPrefab, topPanel.transform);
            topPanelAbilitySlots.Add(singleCell);

            return singleCell;
        }

        public void IncreaseIndexOfCurrentShowingRow()
        {
            currentShowingRow++;
            currentShowingRow = (int)Mathf.Repeat(currentShowingRow, currentRowsQuantity);
            currentPageIndexIndicator.text = (currentShowingRow + 1).ToString();
            UpdateEnabledSlots();
        }

        public void DecreaseIndexOfCurrentShowingRow()
        {
            currentShowingRow--;
            currentShowingRow = (int)Mathf.Repeat(currentShowingRow, currentRowsQuantity);
            currentPageIndexIndicator.text = (currentShowingRow + 1).ToString();
            UpdateEnabledSlots();
        }

        public int SlotsInTheRowQuantity
        {
            get => slotsInTheRowQuantity;

            set
            {
                if (value * currentRowsQuantity < fullSlotsCount)
                    return;

                slotsInTheRowQuantity = value;
                slotsInTheRowQuantity = Mathf.Clamp(slotsInTheRowQuantity, minSlotsInTheRowQuantity, maxSlotsInTheRowQuantity);
                abilityButtonsSlotsPanel.sizeDelta = new Vector2(CalculateWidth(), abilityButtonsSlotsPanel.sizeDelta.y);

                UpdateEnabledSlots();
            }
        }

        public void IncreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity += 1;

        public void DecreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity -= 1;

        private int CalculateWidth() => sizeDelta * slotsInTheRowQuantity;
    }
}