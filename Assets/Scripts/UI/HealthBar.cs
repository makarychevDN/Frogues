using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Stats stats;

        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider preTakenDamageHPSlider;
        [SerializeField] private Slider preTakenDamageAnimatedHPSlider;

        [SerializeField] private TextMeshProUGUI healthTextField;
        [SerializeField] private TextMeshProUGUI blockTextField;
        [SerializeField] private TextMeshProUGUI armorTextField;
        [SerializeField] private TextMeshProUGUI spikesTextField;

        public void SetHealthAndStats(Health health, Stats stats)
        {
            this.health = health;
            this.stats = stats;
        }

        public void SetStats(Stats stats)
        {
            this.stats = stats;
        }

        public void Update()
        {
            int maxSlidersValue = health.MaxHp;
            hpSlider.maxValue = maxSlidersValue;
            preTakenDamageHPSlider.maxValue = maxSlidersValue;
            if(preTakenDamageAnimatedHPSlider != null) preTakenDamageAnimatedHPSlider.maxValue = maxSlidersValue;
            
            hpSlider.value = health.HealthWithPreTakenDamage;
            preTakenDamageHPSlider.value = health.CurrentHp;
            if (preTakenDamageAnimatedHPSlider != null) preTakenDamageAnimatedHPSlider.value = health.CurrentHp;

            if (healthTextField == null)
                return;
          
            healthTextField.text = (health.CurrentHp).ToString();
            blockTextField.text = (health.TemporaryBlock).ToString();
            armorTextField.text = (health.PermanentBlock).ToString();
            spikesTextField.text = (stats.Spikes).ToString();
        }
    }
}