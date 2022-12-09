using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Damagable : MonoBehaviour
    {
        [SerializeField] private int maxHP;
        [SerializeField] private int currentHP;
        [SerializeField] private int armor;
        public UnityEvent OnApplyUnblockedDamage;
        public UnityEvent OnHpEnded;
        private int _healthWithPreTakenDamage;
        private int _hashedHp;
        private AbleToDie _ableToDie;
        private Animator _animator;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;

        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;

        public void Init(Unit unit)
        {
            _ableToDie = unit.AbleToDie;
            _animator = unit.Animator;
        }

        private void Awake()
        {
            _hashedHp = currentHP;
            OnApplyUnblockedDamage.AddListener(TriggerTakeDamageAnimation);
        }
        
        private void TriggerTakeDamageAnimation() => _animator.SetTrigger(CharacterAnimatorParameters.TakeDamage);

        public void TakeHealing(int value)
        {
            currentHP += value;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        }

        public void TakeDamage(int damageValue) =>
            TakeDamage(damageValue, false);

        public void TakeDamage(int damageValue, bool ignoreArmor)
        {
            CalculateDamage(ref currentHP, damageValue, ignoreArmor);

            if (currentHP < _hashedHp)
            {
                OnApplyUnblockedDamage.Invoke();
            }

            if (currentHP <= 0)
            {
                OnHpEnded.Invoke();
                _ableToDie.Die();
            }
            
            _hashedHp = currentHP;
        }

        public void PreTakeDamage(int damageValue) =>
            CalculateDamage(ref _healthWithPreTakenDamage, damageValue, false);

        public void PreTakeDamage(int damageValue, bool ignoreArmor) =>
            CalculateDamage(ref _healthWithPreTakenDamage, damageValue, ignoreArmor);

        private void CalculateDamage(ref int hp, int damageValue, bool ignoreArmor)
        {
            if (!ignoreArmor)
            {
                damageValue -= armor;
                damageValue = Mathf.Clamp(damageValue, 0, 1000);
            }
            
            if(damageValue == 0)
                return;
            
            hp -= damageValue;
        }

        public void ResetPreDamageValue()
        {
            _healthWithPreTakenDamage = currentHP;
        }

        public void DieFromStepOnUnit()
        {
            TakeDamage(maxHP, true);
        }
    }
}