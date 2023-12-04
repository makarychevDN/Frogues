using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descriptionLable;
        [SerializeField] private AbilitiesPanel abilitiesPanel;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private StatsVisualizationSystem statsVisualizationSystem;
        [SerializeField] private ResourcePointsUI resourcePointsUI;

        public void Activate(Unit unit)
        {
            abilitiesPanel.RemoveAllAbilitiesButtons();
            abilitiesPanel.Init(unit);
            abilitiesPanel.AddAbilitiesButtons(unit.AbilitiesManager.Abilities);
            panel.SetActive(true);

            nameLabel.text = unit.UnitDescription.UnitName;
            descriptionLable.text = unit.UnitDescription.Description;
            healthBar.SetHealth(unit.Health);
            statsVisualizationSystem.SetStats(unit.Stats);
            resourcePointsUI.Init(unit.ActionPoints);
        }
    }
}