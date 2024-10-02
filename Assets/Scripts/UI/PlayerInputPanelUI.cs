using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class PlayerInputPanelUI : MonoBehaviour
    {
        [SerializeField] private AbilitiesPanel abilitiesPanel;
        [SerializeField] private AbilitiesPanelHealthBar healthBar;
        [SerializeField] private StatsVisualizationSystem statsVisualizationSystem;
        [SerializeField] private ResourcePointsUI resourcePointsUI;
        [SerializeField] private Button skipTurnButton;
        [SerializeField] private UIOfUnit uIOfUnit;
        [SerializeField] private SlotsPack slotsPack;

        public void Init(Unit unit)
        {
            abilitiesPanel.RemoveAllAbilitiesButtons();
            abilitiesPanel.Init(unit);
            abilitiesPanel.AddAbilitiesButtons(unit.AbilitiesManager.Abilities);

            healthBar.SetHealthAndStats(unit.Health, unit.Stats);
            statsVisualizationSystem.SetStats(unit.Stats);
            resourcePointsUI.Init(unit.ActionPoints);

            skipTurnButton.onClick.AddListener(unit.AbleToSkipTurn.AutoSkip);

            uIOfUnit.Init(unit);
            slotsPack.Init(unit.AbilitiesManager);

            GetComponentsInChildren<EnableButtonOnInputIsPossible>().ToList().ForEach(enabler => enabler.Init(unit.ActionsInput as PlayerInput));
        }
    }
}