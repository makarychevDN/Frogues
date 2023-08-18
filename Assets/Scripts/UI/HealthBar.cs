using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider armorSlider;
        [SerializeField] private Slider emptyHPSlider;
        [SerializeField] private Slider preTakenDamageHPSlider;
        [SerializeField] private Slider preTakenDamageArmorSlider;
        [SerializeField] private Health health;
        [SerializeField] private TextMeshProUGUI textField;

        public void Update()
        {
            int maxSlidersValue = health.MaxHp + health.Block;
            hpSlider.maxValue = maxSlidersValue;
            armorSlider.maxValue = maxSlidersValue;
            emptyHPSlider.maxValue = maxSlidersValue;
            preTakenDamageHPSlider.maxValue = maxSlidersValue;
            preTakenDamageArmorSlider.maxValue = maxSlidersValue;
            
            hpSlider.value = health.HealthWithPreTakenDamage;
            preTakenDamageHPSlider.value = health.CurrentHp;
            emptyHPSlider.value = health.MaxHp;
            armorSlider.value = health.ArmorWithPreTakenDamage + health.MaxHp;
            preTakenDamageArmorSlider.value = health.Block + health.MaxHp;

            if (textField == null)
                return;

            string text = (health.CurrentHp + health.Block).ToString();            
            textField.text = text;
        }
    }
}