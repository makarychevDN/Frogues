using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Damagable health;
        [SerializeField] private TextMeshProUGUI textField;

        public void Update()
        {
            slider.maxValue = health.MaxHp;
            slider.value = health.HealthWithPreTakenDamage;

            if (textField != null)
                textField.text = health.CurrentHp.ToString();
        }
    }
}