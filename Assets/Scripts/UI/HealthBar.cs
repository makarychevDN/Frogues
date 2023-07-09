using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider armorSlider;
        [SerializeField] private Slider preTakenDamageDeltaSlider;
        [SerializeField] private Health health;
        [SerializeField] private TextMeshProUGUI textField;

        public void Update()
        {
            hpSlider.maxValue = health.MaxHp + health.Block;
            armorSlider.maxValue = health.MaxHp + health.Block;
            preTakenDamageDeltaSlider.maxValue = health.MaxHp + health.Block;
            
            hpSlider.value = health.HealthWithPreTakenDamage;
            armorSlider.value = health.HealthWithPreTakenDamage + health.ArmorWithPreTakenDamage;
            preTakenDamageDeltaSlider.value = health.CurrentHp + health.Block;

            if (textField == null)
                return;

            string text = (health.CurrentHp + health.Block).ToString();

            //if (health.Armor != 0)
                //text += $"   <#aac0cd>{health.Armor}</color>";
            
            textField.text = text;
        }
    }
}