using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public abstract class BaseHealthBar : MonoBehaviour
    {
        [SerializeField] protected Health health;
        [SerializeField] protected Stats stats;

        [SerializeField] protected RectTransform resizableParent;

        [SerializeField] protected Slider hpSlider;
        [SerializeField] protected Slider preTakenDamageHPSlider;
        [SerializeField] protected Slider preTakenDamageAnimatedHPSlider;

        public void SetHealthAndStats(Health health, Stats stats)
        {
            this.health = health;
            this.stats = stats;
        }

        public void Update()
        {
            Redraw();
        }

        protected virtual void Redraw()
        {
            hpSlider.maxValue = health.MaxHp;
            preTakenDamageHPSlider.maxValue = health.MaxHp;
            preTakenDamageAnimatedHPSlider.maxValue = health.MaxHp;

            hpSlider.value = health.HealthWithPreTakenDamage;
            preTakenDamageHPSlider.value = health.CurrentHp;
            preTakenDamageAnimatedHPSlider.value = health.CurrentHp;
        }
    }
}