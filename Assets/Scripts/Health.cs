using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Health : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization
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
        [SerializeField] private AudioSource deathFromStepOnThisUnitAudioSource;
        private int _healthWithPreTakenDamage, _permanentArmorWithPreTakenDamage, _temporaryArmorWithPreTakenDamage;
        private int _hashedHp, _hashedArmor;
        private bool _enemy;
        private AbleToDie _ableToDie;
        private Animator _animator;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;
        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;
        public int PermanentArmor => permanentArmor;
        public int TemporaryArmor => temporaryArmor;
        public int Armor => temporaryArmor + permanentArmor;
        public int ArmorWithPreTakenDamage => _temporaryArmorWithPreTakenDamage + _permanentArmorWithPreTakenDamage;

        public bool Full => currentHP == maxHP;

        public void IncreaseTemporaryArmor(int value)
        {
            temporaryArmor += value;
            _hashedArmor = Armor;
            OnTemporaryBlockIncreased.Invoke();
        }
        
        public void IncreasePermanentArmor(int value)
        {
            permanentArmor += value;
            OnPermanentBlockIncreased.Invoke();
        }

        public void Init(Unit unit)
        {
            _ableToDie = unit.AbleToDie;
            _animator = unit.Animator;
            _enemy = unit.Enemy;
            
            _hashedHp = currentHP;
            _hashedArmor = Armor;
            OnApplyUnblockedDamage.AddListener(TriggerTakeDamageAnimation);
            unit.OnStepOnThisUnit.AddListener(DieFromStepOnUnit);
        }

        private void TriggerTakeDamageAnimation()
        {
            CurrentlyActiveObjects.Add(this);
            _animator.SetTrigger(CharacterAnimatorParameters.TakeDamage);
            Invoke("RemoveFromCurrentlyActiveObjects", 0.25f); //todo улучшить эту штуку
        }

        private void RemoveFromCurrentlyActiveObjects()
        {
            CurrentlyActiveObjects.Remove(this);
        }

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

            if (!ignoreArmor)
            {
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
            }

            if (currentHP < _hashedHp)
            {
                CurrentlyActiveObjects.Add(this);
                OnApplyUnblockedDamage.Invoke();
            }

            if (currentHP <= 0)
            {
                Invoke(nameof(DieProcess), 0.25f);
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

        public void DieFromStepOnUnit()
        {
            deathFromStepOnThisUnitAudioSource.Play();
            TakeDamage(maxHP, true);
        }
        
        public void DieFromBumpInto()
        {
            deathFromStepOnThisUnitAudioSource.Play();
            _ableToDie.DieWithoutAnimation();
        }

        public void TickBeforePlayerTurn()
        {
            if(_enemy)
                return;
            
            temporaryArmor = 0;
            _hashedArmor = Armor;
        }

        public void TickBeforeEnemiesTurn()
        {
            if(!_enemy)
                return;
            
            temporaryArmor = 0;
            _hashedArmor = Armor;
        }

        private void DieProcess()
        {
            OnHpEnded.Invoke();
            _ableToDie.Die();
        }

        public void DisablePreVisualization()
        {
            _healthWithPreTakenDamage = currentHP;
            _permanentArmorWithPreTakenDamage = permanentArmor;
            _temporaryArmorWithPreTakenDamage = temporaryArmor;
        }
    }
}