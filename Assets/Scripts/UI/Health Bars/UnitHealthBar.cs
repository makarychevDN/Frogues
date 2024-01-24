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
        [SerializeField] private IntSpriteFontSegment healthTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment blockTextField;
        [SerializeField] private IntSpriteFontSegment blockTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment armorTextField;
        [SerializeField] private IntSpriteFontSegment armorTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment spikesTextField;

        protected override void Redraw()
        {
            base.Redraw();

            blockIcon.SetActive(health.Block != 0);
            armorIcon.SetActive(health.Armor != 0);
            spikesIcon.SetActive(stats.Spikes != 0);

            healthTextField.SetValue(health.HealthWithPreTakenDamage);
            healthTextFieldPretakenAnimation.SetValue(health.HealthWithPreTakenDamage);
            healthTextFieldPretakenAnimation.gameObject.SetActive(health.HealthWithPreTakenDamage != health.CurrentHp);

            armorTextField.SetValue(health.ArmorWithPreTakenDamage);
            armorTextFieldPretakenAnimation.SetValue(health.ArmorWithPreTakenDamage);
            armorTextFieldPretakenAnimation.gameObject.SetActive(health.ArmorWithPreTakenDamage != health.Armor);

            blockTextField.SetValue(health.BlockWithPreTakenDamage);
            blockTextFieldPretakenAnimation.SetValue(health.BlockWithPreTakenDamage);
            blockTextFieldPretakenAnimation.gameObject.SetActive(health.BlockWithPreTakenDamage != health.Block);

            spikesTextField.SetValue(stats.Spikes);

            resizableParents.ForEach(resizableParent => LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent));
        }
    }
}