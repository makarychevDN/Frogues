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

        protected int _hashFuncionOfHealth;
        protected int _hashFuncionOfStats;

        public void SetHealthAndStats(Health health, Stats stats)
        {
            this.health = health;
            this.stats = stats;
        }

        public void Update()
        {
            if(_hashFuncionOfHealth != health.CalculateHashFunctionOfPrevisualisation()
                || _hashFuncionOfStats != stats.CalculateHashFunctionOfPrevisualisation())
            {
                Redraw();
            }

            _hashFuncionOfHealth = health.CalculateHashFunctionOfPrevisualisation();
            _hashFuncionOfStats = stats.CalculateHashFunctionOfPrevisualisation();
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