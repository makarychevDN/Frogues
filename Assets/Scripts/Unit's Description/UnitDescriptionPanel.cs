using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class UnitDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform parentOfContent;
        [SerializeField] private List<RectTransform> resizableContent;
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
            parentOfContent.gameObject.SetActive(enabled);
            resizableContent.ForEach(content => LayoutRebuilder.ForceRebuildLayoutImmediate(content));

            nameLabel.text = unit.UnitDescription.UnitName;
            descriptionLable.text = unit.UnitDescription.Description;
            healthBar.SetHealth(unit.Health);
            statsVisualizationSystem.SetStats(unit.Stats);
            resourcePointsUI.Init(unit.ActionPoints);
        }

        private void ForceUpdateCanvases() => Canvas.ForceUpdateCanvases();

        public void EnableContent(bool enabled)
        {
            parentOfContent.gameObject.SetActive(enabled);
        }

        private void EnableAbilitiesPanel() => abilitiesPanel.gameObject.SetActive(true);
    }
}