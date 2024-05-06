using UnityEngine.UI;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitHealthBar : BaseHealthBar
    {
        [SerializeField] private GameObject blockIcon;
        [SerializeField] private GameObject armorIcon;
        [SerializeField] private GameObject spikesIcon;
        [SerializeField] private GameObject escapeFromDeathIcon;

        [SerializeField] private IntSpriteFontSegment healthTextField;
        [SerializeField] private IntSpriteFontSegment healthTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment blockTextField;
        [SerializeField] private IntSpriteFontSegment blockTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment armorTextField;
        [SerializeField] private IntSpriteFontSegment armorTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment escapeFromDeathIconTextField;
        [SerializeField] private IntSpriteFontSegment escapeFromDeathIconTextFieldPretakenAnimation;

        [SerializeField] private IntSpriteFontSegment spikesTextField;

        public override void Redraw()
        {
            base.Redraw();

            blockIcon.SetActive(health.Block != 0);
            armorIcon.SetActive(health.Armor != 0);
            spikesIcon.SetActive(stats.Thorns != 0);
            escapeFromDeathIcon.SetActive(health.EscapesFromDeath != 0);

            healthTextField.SetValue(health.HealthWithPreTakenDamage);
            healthTextFieldPretakenAnimation.SetValue(health.HealthWithPreTakenDamage);
            healthTextFieldPretakenAnimation.gameObject.SetActive(health.HealthWithPreTakenDamage != health.CurrentHp);

            armorTextField.SetValue(health.ArmorWithPreTakenDamage);
            armorTextFieldPretakenAnimation.SetValue(health.ArmorWithPreTakenDamage);
            armorTextFieldPretakenAnimation.gameObject.SetActive(health.ArmorWithPreTakenDamage != health.Armor);

            blockTextField.SetValue(health.BlockWithPreTakenDamage);
            blockTextFieldPretakenAnimation.SetValue(health.BlockWithPreTakenDamage);
            blockTextFieldPretakenAnimation.gameObject.SetActive(health.BlockWithPreTakenDamage != health.Block);

            escapeFromDeathIconTextField.SetValue(health.EscapesFromDeathCountWithPretakenDamage);
            escapeFromDeathIconTextFieldPretakenAnimation.SetValue(health.EscapesFromDeathCountWithPretakenDamage);
            escapeFromDeathIconTextFieldPretakenAnimation.gameObject.SetActive(health.EscapesFromDeathCountWithPretakenDamage != health.EscapesFromDeath);

            spikesTextField.SetValue(stats.Thorns);

            resizableParents.ForEach(resizableParent => LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent));
        }
    }
}