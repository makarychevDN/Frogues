using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;

namespace FroguesFramework
{
    public class Health : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization
    {
        [SerializeField] private int maxHP;
        [SerializeField] private int currentHP;
        [SerializeField] private int permanentBlock;
        [SerializeField] private int temporaryBlock;
        public UnityEvent OnApplyUnblockedDamage;
        public UnityEvent OnDamageBlockedSuccessfully;
        public UnityEvent OnBlockDestroyed;
        public UnityEvent OnTemporaryBlockIncreased;
        public UnityEvent OnPermanentBlockIncreased;
        public UnityEvent OnBlockIncreased;
        public UnityEvent OnHpEnded;
        [SerializeField] private AudioSource deathFromStepOnThisUnitAudioSource;
        private int _healthWithPreTakenDamage, _permanentArmorWithPreTakenDamage, _temporaryArmorWithPreTakenDamage;
        private int _hashedHp, _hashedBlock;
        private Unit _unit;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;
        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;
        public int PermanentBlock => permanentBlock;
        public int TemporaryBlock => temporaryBlock;
        public int Block => temporaryBlock + permanentBlock;
        public int ArmorWithPreTakenDamage => _temporaryArmorWithPreTakenDamage + _permanentArmorWithPreTakenDamage;

        public bool Full => currentHP == maxHP;

        public void IncreaseTemporaryBlock(int value)
        {
            temporaryBlock += (int)(value * _unit.Stats.DefenceModificator);
            _hashedBlock = Block;
            OnTemporaryBlockIncreased.Invoke();
            OnBlockIncreased.Invoke();
        }
        
        public void IncreasePermanentBlock(int value)
        {
            permanentBlock += (int)(value * _unit.Stats.DefenceModificator);
            permanentBlock += value;
            _hashedBlock = Block;
            OnPermanentBlockIncreased.Invoke();
            OnBlockIncreased.Invoke();
        }

        public void Init(Unit unit)
        {
            _unit = unit;            
            _hashedHp = currentHP;
            _hashedBlock = Block;
            OnApplyUnblockedDamage.AddListener(TriggerTakeDamageAnimation);
            unit.OnStepOnThisUnit.AddListener(DieFromStepOnUnit);
            AddMySelfToEntryPoint();
        }

        private void Update()
        {
            _hashedBlock = Block;
            _hashedHp = currentHP;
        }

        private void TriggerTakeDamageAnimation()
        {
            CurrentlyActiveObjects.Add(this);
            _unit.Animator.SetTrigger(CharacterAnimatorParameters.TakeDamage);
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
            CalculateDamage(ref currentHP, ref permanentBlock,ref temporaryBlock, damageValue, ignoreArmor);

            if (!ignoreArmor)
            {
                if (_hashedBlock != 0)
                {
                    if (Block != 0)
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
            _hashedBlock = Block;
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
            _unit.AbleToDie.DieWithoutAnimation();
        }

        public void TickBeforePlayerTurn()
        {
            if(_unit.IsEnemy)
                return;
            
            temporaryBlock = 0;
            _hashedBlock = Block;
        }

        public void TickBeforeEnemiesTurn()
        {
            if(!_unit.IsEnemy)
                return;
            
            temporaryBlock = 0;
            _hashedBlock = Block;
        }

        private void DieProcess()
        {
            OnHpEnded.Invoke();
            _unit.AbleToDie.Die();
        }

        public void DisablePreVisualization()
        {
            _healthWithPreTakenDamage = currentHP;
            _permanentArmorWithPreTakenDamage = permanentBlock;
            _temporaryArmorWithPreTakenDamage = temporaryBlock;
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);
    }
}