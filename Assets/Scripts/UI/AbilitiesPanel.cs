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
        [SerializeField] private Transform parentToDragAndDrop;
        [SerializeField] private List<AbilityButtonSlot> bottomPanelAbilitySlots;
        [SerializeField] private List<AbilityButtonSlot> topPanelAbilitySlots;
        [SerializeField] private bool enableHotKeys;
        [SerializeField] private bool itsButtonsAreInteractive;
        [SerializeField] private bool ableToGiveLastEmptySlot;
        [SerializeField] private Vector2 positionOfHintRelativeToButtonInBottomPanel;
        [SerializeField] private Vector2 pivotOfHintRectTransformWhenHoverInBottomPanel;
        [SerializeField] private Vector2 positionOfHintRelativeToButtonInTopPanel;
        [SerializeField] private Vector2 pivotOfHintRectTransformWhenHoverInTopPanel;

        [Header("Panel Size Controller Setup")]
        [SerializeField] private RectTransform abilityButtonsSlotsPanel;
        [SerializeField] private List<RectTransform> abilitySlotsBorders;
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

        [SerializeField] private int currentRowsQuantity;
        private int abilitySlotBorderHeight = 46;
        private float maxDistanceToClamp = 64;
        private Dictionary<int, KeyCode> keyCodesByInt = new Dictionary<int, KeyCode>
        {
            { 0, KeyCode.Alpha1},
            { 1, KeyCode.Alpha2},
            { 2, KeyCode.Alpha3},
            { 3, KeyCode.Alpha4},
            { 4, KeyCode.Alpha5},
            { 5, KeyCode.Alpha6},
            { 6, KeyCode.Alpha7},
            { 7, KeyCode.Alpha8},
            { 8, KeyCode.Alpha9},
            { 9, KeyCode.Alpha0},
            { 10, KeyCode.Minus},
            { 11, KeyCode.Equals}
        };

        public AbilitiesManager AbilitiesManager => abilitiesManager;
        public List<AbilityButtonSlot> ActiveAbilitySlots => bottomPanelAbilitySlots;
        public List<AbilityButtonSlot> PassiveAbilitySlots => topPanelAbilitySlots;

        public void Init(Unit unit)
        {
            abilitiesManager?.AbilityHasBeenAdded.RemoveListener(AddAbilityButton);
            abilitiesManager?.AbilityHasBeenRemoved.RemoveListener(RemoveAbilityButton);

            abilitiesManager = unit.AbilitiesManager;

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

            abilitySlotsBorders.ForEach(border => border.sizeDelta = new Vector2(border.sizeDelta.x, showThemAll ? abilitySlotBorderHeight * currentRowsQuantity : abilitySlotBorderHeight));

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

            foreach(var slot in bottomPanelAbilitySlots)
            {
                slot.HotKey.EnableHotKey(false);
            }

            if (!enableHotKeys || showThemAll)
                return;

            int count = 0;
            foreach(var slot in bottomPanelAbilitySlots.Where(slot => slot.gameObject.activeSelf))
            {
                slot.HotKey.EnableHotKey(true);
                slot.HotKey.SetKeyCode(keyCodesByInt[count]);
                count++;

                if (count > 11)
                    return;
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

        public void AddAbilitiesButtons(List<BaseAbility> abilities)
        {
            foreach(var ability in abilities)
            {
                AddAbilityButton(ability);
            }
        }

        private void AddAbilityButton(BaseAbility ability)
        {
            var abilityAsAbleToDrawAbilityButton = ability as IAbleToDrawAbilityButton;

            if (abilityAsAbleToDrawAbilityButton == null || abilityAsAbleToDrawAbilityButton.IsIgnoringDrawingFunctionality())
                return;

            AbilityButtonSlot slot;
            Vector2 pivot;
            Vector2 relatiovePosition;

            if (topPanel != null && ability is PassiveAbility)
            {
                slot = AddTopAbilitySlot();
                pivot = pivotOfHintRectTransformWhenHoverInTopPanel;
                relatiovePosition = positionOfHintRelativeToButtonInTopPanel;
            }
            else
            {
                slot = ableToGiveLastEmptySlot && ability is InspectAbility ? GetLastEmptyEnabledSlotInBottomPanel() : GetFirstEmptySlotInBottonPanel();
                pivot = pivotOfHintRectTransformWhenHoverInBottomPanel;
                relatiovePosition = positionOfHintRelativeToButtonInBottomPanel;
            }

            var abilityButton = Instantiate(abilityButtonPrefab, transform, true);
            abilityButton.Init(ability, slot, itsButtonsAreInteractive, true, pivot, relatiovePosition, parentToDragAndDrop);
            abilityButton.OnAbilityPicked.AddListener(setCurrentAbility);
            abilityButton.OnDropButton.AddListener(PlaceButtonInTheClosetstClot);
        }

        private void setCurrentAbility(AbleToUseAbility ability)
        {
            if (abilitiesManager.AbleToHaveCurrentAbility == null)
                return;

            if (abilitiesManager.AbleToHaveCurrentAbility.GetCurrentAbility() == ability)
                abilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();

            else
                abilitiesManager.AbleToHaveCurrentAbility.SetCurrentAbility(ability);
        }

        private void PlaceButtonInTheClosetstClot(AbilityButton abilityButton)
        {
            var closestSlot = abilityButton.AbilityButtonSlot;
            var abilitiesSlots = topPanel != null && abilityButton.Ability is PassiveAbility ? PassiveAbilitySlots : ActiveAbilitySlots;

            foreach (var temp in abilitiesSlots)
            {
                if (Vector3.Distance(closestSlot.transform.position, abilityButton.transform.position) >
                    Vector3.Distance(temp.transform.position, abilityButton.transform.position))
                    closestSlot = temp;
            }

            if (Vector3.Distance(closestSlot.transform.position, abilityButton.transform.position) < maxDistanceToClamp)
            {
                abilityButton.AbilityButtonSlot.Clear();

                if (!closestSlot.Empty)
                {
                    closestSlot.AbilityButton.SetSlot(abilityButton.AbilityButtonSlot);
                }

                abilityButton.AbilityButtonSlot = closestSlot;
            }

            abilityButton.AbilityButtonSlot.AddButton(abilityButton);
        }

        private void RemoveAbilityButton(BaseAbility ability)
        {
            foreach (var abilitySlot in bottomPanelAbilitySlots)
            {
                var button = abilitySlot.AbilityButton;

                if (button == null || button.Ability != ability)
                    continue;

                DestroyImmediate(button.gameObject);
                return;
            }

            foreach (var abilitySlot in topPanelAbilitySlots)
            {
                var button = abilitySlot.AbilityButton;

                if (button == null || button.Ability != ability)
                    continue;

                PassiveAbilitySlots.Remove(abilitySlot);
                DestroyImmediate(abilitySlot.gameObject);
                return;
            }
        }

        public void RemoveAllAbilitiesButtons()
        {
            foreach (var abilitySlot in bottomPanelAbilitySlots)
            {
                if (abilitySlot.Empty)
                    continue;

                var button = abilitySlot.AbilityButton;
                abilitySlot.Clear();
                if(button.Ability is AbleToUseAbility)
                    button.OnAbilityPicked.RemoveListener(setCurrentAbility);
                button.OnDropButton.RemoveListener(PlaceButtonInTheClosetstClot);
                DestroyImmediate(button.gameObject);
            }

            while(topPanelAbilitySlots.Count > 0)
            {
                var slot = topPanelAbilitySlots[0];;
                slot.Clear();
                PassiveAbilitySlots.Remove(slot);
                DestroyImmediate(slot.gameObject);
            }

            UpdateEnabledSlots();
        }

        private AbilityButtonSlot GetFirstEmptySlotInBottonPanel()
        {
            if (bottomPanelAbilitySlots.None(slot => slot.Empty) || (slotsInTheRowQuantity * currentRowsQuantity < (fullSlotsCount + 1)))
            {
                currentRowsQuantity++;
                UpdateEnabledSlots();
            }

            return bottomPanelAbilitySlots.First(slot => slot.Empty);
        }

        private AbilityButtonSlot GetLastEmptyEnabledSlotInBottomPanel()
        {
            return bottomPanelAbilitySlots.LastOrDefault(slot => slot.gameObject.activeInHierarchy && slot.Empty);
        }

        private AbilityButtonSlot AddTopAbilitySlot()
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