using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public abstract class BaseHealthBar : MonoBehaviour
    {
        [SerializeField] protected Health health;
        [SerializeField] protected Stats stats;

        [SerializeField] protected List<RectTransform> resizableParents;

        [SerializeField] protected Slider hpSlider;
        [SerializeField] protected Slider preTakenDamageHPSlider;
        [SerializeField] protected Slider preTakenDamageAnimatedHPSlider;

        protected int _hashFuncionOfHealth;
        protected int _hashFuncionOfStats;
        protected int _hashFuncionOfActionPoints;

        public void SetHealthAndStats(Health health, Stats stats)
        {
            this.health = health;
            this.stats = stats;            
        }

        void OnEnable()
        {
            StartCoroutine(RedrawOnSecondFrame());
        }

        IEnumerator RedrawOnSecondFrame()
        {
            //returning 0 will make it wait 1 frame
            yield return 0;
            Redraw();
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
            preTakenDamageAnimatedHPSlider.gameObject.SetActive(health.CurrentHp != health.HealthWithPreTakenDamage);
        }
    }
}