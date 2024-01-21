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

        protected override void Redraw()
        {
            base.Redraw();

            blockIcon.SetActive(health.TemporaryBlock != 0);
            armorIcon.SetActive(health.PermanentBlock != 0);
            spikesIcon.SetActive(stats.Spikes != 0);

            healthTextField.text = (health.CurrentHp).ToString();
            blockTextField.text = (health.TemporaryBlock).ToString();
            armorTextField.text = (health.PermanentBlock).ToString();
            spikesTextField.text = (stats.Spikes).ToString();

            resizableParents.ForEach(resizableParent => LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent));
        }
    }
}