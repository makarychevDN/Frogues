using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Damagable : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private int maxHP;
        [SerializeField] private int currentHP;
        [SerializeField] private int permanentArmor;
        [SerializeField] private int temporaryArmor;
        public UnityEvent OnApplyUnblockedDamage;
        public UnityEvent OnDamageBlockedSuccessfully;
        public UnityEvent OnBlockDestroyed;
        public UnityEvent OnTemporaryBlockIncreased;
        public UnityEvent OnPermanentBlockIncreased;
        public UnityEvent OnBlockIncreased;
        public UnityEvent OnHpEnded;
        private int _healthWithPreTakenDamage, _permanentArmorWithPreTakenDamage, _temporaryArmorWithPreTakenDamage;
        private int _hashedHp, _hashedArmor;
        private AbleToDie _ableToDie;
        private Animator _animator;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;
        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;
        public int PermanentArmor => permanentArmor;
        public int TemporaryArmor => temporaryArmor;
        public int Armor => temporaryArmor + permanentArmor;
        public int ArmorWithPreTakenDamage => _temporaryArmorWithPreTakenDamage + _permanentArmorWithPreTakenDamage;

        public void IncreaseTemporaryArmor(int value)
        {
            temporaryArmor += value;
            OnTemporaryBlockIncreased.Invoke();
        }

        public void Init(Unit unit)
        {
            _ableToDie = unit.AbleToDie;
            _animator = unit.Animator;
        }

        private void Awake()
        {
            _hashedHp = currentHP;
            _hashedArmor = Armor;
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
            CalculateDamage(ref currentHP, ref permanentArmor,ref temporaryArmor, damageValue, ignoreArmor);

            if (_hashedArmor != 0)
            {
                if (Armor != 0)
                {
                    OnDamageBlockedSuccessfully.Invoke();
                }
                else
                {
                    OnBlockDestroyed.Invoke();
                }
            }
            
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
            _hashedArmor = Armor;
        }

        public void PreTakeDamage(int damageValue) =>
            CalculateDamage(ref _healthWithPreTakenDamage, ref _permanentArmorWithPreTakenDamage,
                ref _temporaryArmorWithPreTakenDamage, damageValue, false);

        public void PreTakeDamage(int damageValue, bool ignoreArmor) =>
            CalculateDamage(ref _healthWithPreTakenDamage, ref _permanentArmorWithPreTakenDamage,
                ref _temporaryArmorWithPreTakenDamage, damageValue, ignoreArmor);

        private void CalculateDamage(ref int calculatingHp, ref int calculatingPermanentArmor, ref int calculatingTemporaryArmor, int damageValue, bool ignoreArmor)
        {
            if (!ignoreArmor)
            {
                int damageToTemporaryArmor = Mathf.Clamp(damageValue, 0, calculatingTemporaryArmor);
                damageValue -= damageToTemporaryArmor;
                calculatingTemporaryArmor -= damageToTemporaryArmor;

                int damageToArmor = Mathf.Clamp(damageValue, 0, calculatingPermanentArmor);
                damageValue -= damageToArmor;
                calculatingPermanentArmor -= damageToArmor;
                
                damageValue = Mathf.Clamp(damageValue, 0, 1000);
            }

            calculatingHp -= damageValue;
        }

        public void ResetPreDamageValue()
        {
            _healthWithPreTakenDamage = currentHP;
            _permanentArmorWithPreTakenDamage = permanentArmor;
            _temporaryArmorWithPreTakenDamage = temporaryArmor;
        }

        public void DieFromStepOnUnit()
        {
            TakeDamage(maxHP, true);
        }

        public void Tick()
        {
            temporaryArmor = 0;
            _hashedArmor = Armor;
        }
    }
}