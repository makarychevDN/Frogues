using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider armorSlider;
        [SerializeField] private Slider preTakenDamageHPSlider;
        [SerializeField] private Slider preTakenDamageArmorSlider;
        [SerializeField] private Health health;
        [SerializeField] private TextMeshProUGUI textField;

        public void Update()
        {
            hpSlider.maxValue = health.MaxHp + health.Block;
            armorSlider.maxValue = health.MaxHp + health.Block;
            preTakenDamageHPSlider.maxValue = health.MaxHp + health.Block;
            preTakenDamageArmorSlider.maxValue = health.MaxHp + health.Block;
            
            hpSlider.value = health.HealthWithPreTakenDamage;
            armorSlider.value = health.ArmorWithPreTakenDamage;
            preTakenDamageHPSlider.value = health.CurrentHp;
            preTakenDamageArmorSlider.value = health.Block;

            if (textField == null)
                return;

            string text = (health.CurrentHp + health.Block).ToString();

            //if (health.Armor != 0)
                //text += $"   <#aac0cd>{health.Armor}</color>";
            
            textField.text = text;
        }
    }
}