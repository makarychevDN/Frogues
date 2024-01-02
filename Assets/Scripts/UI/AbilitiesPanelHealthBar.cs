using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesPanelHealthBar : BaseHealthBar
    {
        [SerializeField] private TextMeshProUGUI healthTextField;
        [SerializeField] private TextMeshProUGUI blockTextField;
        [SerializeField] private TextMeshProUGUI armorTextField;
        [SerializeField] private TextMeshProUGUI spikesTextField;

        protected override void Redraw()
        {
            base.Redraw();
            healthTextField.text = (health.CurrentHp).ToString();
            blockTextField.text = (health.TemporaryBlock).ToString();
            armorTextField.text = (health.PermanentBlock).ToString();
            spikesTextField.text = (stats.Spikes).ToString();
        }
    }
}