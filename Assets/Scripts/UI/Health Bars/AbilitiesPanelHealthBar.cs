using System.Text;
using TMPro;
using UnityEngine;
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

        [SerializeField] private AbilityDescriptionTag blockMechanicDescription;
        [SerializeField] private AbilityDescriptionTag armorMechanicDescription;
        [SerializeField] private AbilityDescriptionTag spikesMechanicDescription;

        protected override void Redraw()
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

        public void ShowHealthHint() => ShowHint("Здоровье", GenerateHealthStatsString(), transform);
        public void ShowBlockHint() => ShowHint("Блок", blockMechanicDescription.DescriptionText, blockIcon.transform);
        public void ShowArmorHint() => ShowHint("Броня", armorMechanicDescription.DescriptionText, spikesIcon.transform);
        public void ShowSpikesHint() => ShowHint("Шипы", spikesMechanicDescription.DescriptionText, spikesIcon.transform);

        private string GenerateHealthStatsString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Текущий запас: {health.CurrentHp}")
                .AppendLine($"Максимальный запас: {health.MaxHp}");

            return sb.ToString();
        }

        private void ShowHint(string header, string descriptionTag, Transform transformOfIcon)
        {
            EntryPoint.Instance.AbilityHint.Init(header, descriptionTag, "", transformOfIcon, new Vector2(0.5f, 0), Vector2.up * 36);
            EntryPoint.Instance.AbilityHint.EnableContent(true, true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}