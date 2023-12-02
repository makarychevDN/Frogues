using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescriptionPanel : MonoBehaviour, IAbleToDisablePreVisualization
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descriptionLable;
        [SerializeField] private AbilitiesPanel abilitiesPanel;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private StatsVisualizationSystem statsVisualizationSystem;
        [SerializeField] private ResourcePointsUI resourcePointsUI;

        private void Start()
        {
            AddMySelfToEntryPoint();
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public void Activate(Unit unit)
        {
            panel.SetActive(true);
            abilitiesPanel.Init(unit);
            nameLabel.text = unit.UnitDescription.UnitName;
            descriptionLable.text = unit.UnitDescription.Description;
            healthBar.SetHealth(unit.Health);
            statsVisualizationSystem.SetStats(unit.Stats);
            resourcePointsUI.Init(unit.ActionPoints);
        }

        public void DisablePreVisualization()
        {
            panel.SetActive(false);
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);
    }
}