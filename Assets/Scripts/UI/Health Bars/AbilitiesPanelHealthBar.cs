using System.Collections.Generic;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilitiesPanelHealthBar : BaseHealthBar
    {
        [SerializeField] private GameObject blockIcon;
        [SerializeField] private GameObject armorIcon;
        [SerializeField] private GameObject spikesIcon;

        [SerializeField] private TextMeshProUGUI healthTextField;
        [SerializeField] private TextMeshProUGUI blockTextField;
        [SerializeField] private TextMeshProUGUI armorTextField;
        [SerializeField] private TextMeshProUGUI spikesTextField;

        [SerializeField] private LocalizedString healthMechanicName;
        [SerializeField] private LocalizedString blockMechanicName;
        [SerializeField] private LocalizedString armorMechanicName;
        [SerializeField] private LocalizedString thornsMechanicName;

        [SerializeField] private AbilityDescriptionTag healthMechanicDescription;
        [SerializeField] private AbilityDescriptionTag blockMechanicDescription;
        [SerializeField] private AbilityDescriptionTag armorMechanicDescription;
        [SerializeField] private AbilityDescriptionTag thornsMechanicDescription;

        private Dictionary<string, Func<string>> _dataByKeyWords = new Dictionary<string, Func<string>>();

        private void Awake()
        {
            _dataByKeyWords.Add("{health}", () => health.CurrentHp.ToString());
            _dataByKeyWords.Add("{max_health}", () => health.MaxHp.ToString());
        }

        public override void Redraw()
        {
            base.Redraw();

            blockIcon.SetActive(health.Block != 0);
            armorIcon.SetActive(health.Armor != 0);
            spikesIcon.SetActive(stats.Spikes != 0);

            healthTextField.text = (health.CurrentHp).ToString();
            blockTextField.text = (health.Block).ToString();
            armorTextField.text = (health.Armor).ToString();
            spikesTextField.text = (stats.Spikes).ToString();

            resizableParents.ForEach(resizableParent => LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent));
        }

        public void ShowHealthHint() => ShowHint(healthMechanicName.GetLocalizedString(), GenerateHealthStatsString(healthMechanicDescription), transform);
        public void ShowBlockHint() => ShowHint(blockMechanicName.GetLocalizedString(), blockMechanicDescription.DescriptionText, blockIcon.transform);
        public void ShowArmorHint() => ShowHint(armorMechanicName.GetLocalizedString(), armorMechanicDescription.DescriptionText, spikesIcon.transform);
        public void ShowSpikesHint() => ShowHint(thornsMechanicName.GetLocalizedString(), thornsMechanicDescription.DescriptionText, spikesIcon.transform);

        private string GenerateHealthStatsString(AbilityDescriptionTag healthMechanicDescription)
        {
            return Extensions.GenerateDescription(new List<AbilityDescriptionTag> { healthMechanicDescription }, _dataByKeyWords, false);
        }

        private void ShowHint(string header, string descriptionTag, Transform transformOfIcon)
        {
            EntryPoint.Instance.AbilityHint.Init(header, descriptionTag, transformOfIcon, new Vector2(0.5f, 0), Vector2.up * 36);
            EntryPoint.Instance.AbilityHint.EnableContent(true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}