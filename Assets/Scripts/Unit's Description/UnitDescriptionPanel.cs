using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class UnitDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject parentOfContent;
        [SerializeField] private GameObject background;
        [SerializeField] private List<RectTransform> resizableContent;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descriptionLable;
        [SerializeField] private AbilitiesPanel abilitiesPanel;
        [SerializeField] private AbilitiesPanelHealthBar healthBar;
        [SerializeField] private StatsVisualizationSystem statsVisualizationSystem;
        [SerializeField] private ResourcePointsUI resourcePointsUI;

        public void Activate(Unit unit)
        {
            abilitiesPanel.RemoveAllAbilitiesButtons();
            abilitiesPanel.Init(unit);
            abilitiesPanel.AddAbilitiesButtons(unit.AbilitiesManager.Abilities);
            EnableContent(true);
            resizableContent.ForEach(content => LayoutRebuilder.ForceRebuildLayoutImmediate(content));

            nameLabel.text = unit.UnitDescription.UnitName;
            descriptionLable.text = unit.UnitDescription.Description;
            healthBar.SetHealthAndStats(unit.Health, unit.Stats);
            statsVisualizationSystem.SetStats(unit.Stats);
            resourcePointsUI.Init(unit.ActionPoints);
        }

        public void EnableContent(bool enabled)
        {
            parentOfContent.SetActive(enabled);
            background.SetActive(enabled);
        }

        public bool IsActive => parentOfContent.activeSelf; 

    }
}