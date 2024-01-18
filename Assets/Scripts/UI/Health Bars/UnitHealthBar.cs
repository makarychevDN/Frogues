using UnityEngine.UI;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitHealthBar : BaseHealthBar
    {
        [SerializeField] private GameObject blockIcon;
        [SerializeField] private GameObject armorIcon;
        [SerializeField] private GameObject spikesIcon;

        [SerializeField] private IntSpriteFontSegment healthTextField;
        [SerializeField] private IntSpriteFontSegment blockTextField;
        [SerializeField] private IntSpriteFontSegment armorTextField;
        [SerializeField] private IntSpriteFontSegment spikesTextField;

        protected override void Redraw()
        {
            base.Redraw();

            blockIcon.SetActive(health.TemporaryBlock != 0);
            armorIcon.SetActive(health.PermanentBlock != 0);
            spikesIcon.SetActive(stats.Spikes != 0);

            healthTextField.SetValue(health.CurrentHp);
            blockTextField.SetValue(health.TemporaryBlock);
            armorTextField.SetValue(health.PermanentBlock);
            spikesTextField.SetValue(stats.Spikes);

            LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent);
        }
    }
}