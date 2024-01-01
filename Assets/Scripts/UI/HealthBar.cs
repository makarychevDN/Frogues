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
        [SerializeField] private Slider preTakenDamageAnimatedHPSlider;
        [SerializeField] private Slider preTakenDamageArmorSlider;
        [SerializeField] private Health health;
        [SerializeField] private TextMeshProUGUI textField;

        public void SetHealth(Health health)
        {
            this.health = health;
        }

        public void Update()
        {
            int maxSlidersValue = health.MaxHp + health.Block;
            hpSlider.maxValue = maxSlidersValue;
            armorSlider.maxValue = maxSlidersValue;
            emptyHPSlider.maxValue = maxSlidersValue;
            preTakenDamageHPSlider.maxValue = maxSlidersValue;
            if(preTakenDamageAnimatedHPSlider != null) preTakenDamageAnimatedHPSlider.maxValue = maxSlidersValue;
            preTakenDamageArmorSlider.maxValue = maxSlidersValue;
            
            hpSlider.value = health.HealthWithPreTakenDamage;
            preTakenDamageHPSlider.value = health.CurrentHp;
            if (preTakenDamageAnimatedHPSlider != null) preTakenDamageAnimatedHPSlider.value = health.CurrentHp;
            emptyHPSlider.value = health.MaxHp;
            armorSlider.value = health.BlockWithPreTakenDamage + health.MaxHp;
            preTakenDamageArmorSlider.value = health.Block + health.MaxHp;

            if (textField == null)
                return;

            string text = (health.CurrentHp + health.Block).ToString();            
            textField.text = text;
        }
    }
}