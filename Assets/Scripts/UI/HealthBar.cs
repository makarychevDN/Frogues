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
        [SerializeField] private Damagable health;
        [SerializeField] private TextMeshProUGUI textField;

        public void Update()
        {
            hpSlider.maxValue = health.MaxHp + health.Armor;
            armorSlider.maxValue = health.MaxHp + health.Armor;
            preTakenDamageDeltaSlider.maxValue = health.MaxHp + health.Armor;
            
            hpSlider.value = health.HealthWithPreTakenDamage;
            armorSlider.value = health.HealthWithPreTakenDamage + health.ArmorWithPreTakenDamage;
            preTakenDamageDeltaSlider.value = health.CurrentHp + health.Armor;

            if (textField == null)
                return;

            string text = (health.CurrentHp + health.Armor).ToString();

            //if (health.Armor != 0)
                //text += $"   <#aac0cd>{health.Armor}</color>";
            
            textField.text = text;
        }
    }
}