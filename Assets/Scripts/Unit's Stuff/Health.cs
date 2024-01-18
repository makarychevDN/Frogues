using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Health : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization, IAbleToCalculateHashFunctionOfPrevisualisation
    {
        [SerializeField] private int maxHP;
        [SerializeField] private int currentHP;
        [SerializeField] private int permanentBlock;
        [SerializeField] private int temporaryBlock;
        [SerializeField] private int escapesFromDeathCount;
        [SerializeField] private bool dieImmedeatlyAfterStepOnItByUnit;
        public UnityEvent OnApplyUnblockedDamage;
        public UnityEvent OnDamageBlockedSuccessfully;
        public UnityEvent<Unit> OnDamageFromUnitBlockedSuccessfully;
        public UnityEvent OnBlockDestroyed;
        public UnityEvent OnTemporaryBlockIncreased;
        public UnityEvent OnPermanentBlockIncreased;
        public UnityEvent OnBlockIncreased;
        public UnityEvent OnHpEnded;
        public UnityEvent OnHpHealed;
        public UnityEvent OnEscapedFromDeath;
        [SerializeField] private AudioSource deathFromStepOnThisUnitAudioSource;
        private int _healthWithPreTakenDamage, _permanentBlockrWithPreTakenDamage, _temporaryBlockWithPreTakenDamage;
        private int _hashedHp, _hashedBlock;
        private Unit _unit;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;
        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;
        public int PermanentBlock => permanentBlock;
        public int TemporaryBlock => temporaryBlock;
        public int Block => temporaryBlock + permanentBlock;
        public int BlockWithPreTakenDamage => _temporaryBlockWithPreTakenDamage + _permanentBlockrWithPreTakenDamage;

        public bool Full => currentHP == maxHP;

        public void Init(Unit unit)
        {
            _unit = unit;
            _hashedHp = currentHP;
            _hashedBlock = Block;
            OnApplyUnblockedDamage.AddListener(TriggerTakeDamageAnimation);
            unit.OnStepOnThisUnit.AddListener(DieFromStepOnUnit);
            AddMySelfToEntryPoint();
        }

        public void RemoveAllBlockEffects()
        {
            temporaryBlock = 0;
            permanentBlock = 0;
        }

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
            _hashedBlock = Block;
            OnPermanentBlockIncreased.Invoke();
            OnBlockIncreased.Invoke();
        }

        public void IncreaseMaxHp(int value)
        {
            maxHP += value;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        }

        public void IncreaseEscapesFromDeathCount(int value)
        {
            escapesFromDeathCount += value;
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
            OnHpHealed.Invoke();
        }

        public void TakeDamage(int damageValue, Unit damageSource) =>
            TakeDamage(damageValue, false, damageSource);

        public void TakeDamage(int damageValue, bool ignoreBlock, Unit damageSource)
        {
            CalculateDamage(ref currentHP, ref permanentBlock, ref temporaryBlock, damageValue, ignoreBlock);

            if (!ignoreBlock)
            {
                if (_hashedBlock != 0)
                {
                    if (Block != 0)
                    {
                        OnDamageBlockedSuccessfully.Invoke();
                        OnDamageFromUnitBlockedSuccessfully.Invoke(damageSource);
                    }
                    else
                    {
                        OnBlockDestroyed.Invoke();
                    }
                }
            }

            if(damageSource != null && _unit.Stats.Spikes > 0)
            {
                damageSource.Health.TakeDamage(_unit.Stats.Spikes, null);
            }

            if (currentHP < _hashedHp)
            {
                CurrentlyActiveObjects.Add(this);
                OnApplyUnblockedDamage.Invoke();
            }

            if (currentHP <= 0)
            {
                OnHpEnded.Invoke();

                if (escapesFromDeathCount <= 0)
                {
                    Invoke(nameof(DieProcess), 0.25f);
                }
                else
                {
                    OnEscapedFromDeath.Invoke();
                    currentHP = maxHP / 2;
                    escapesFromDeathCount--;
                }
            }
            
            _hashedHp = currentHP;
            _hashedBlock = Block;
        }

        public void PreTakeDamage(int damageValue) =>
            CalculateDamage(ref _healthWithPreTakenDamage, ref _permanentBlockrWithPreTakenDamage,
                ref _temporaryBlockWithPreTakenDamage, damageValue, false);

        public void PreTakeDamage(int damageValue, bool ignoreBlock) =>
            CalculateDamage(ref _healthWithPreTakenDamage, ref _permanentBlockrWithPreTakenDamage,
                ref _temporaryBlockWithPreTakenDamage, damageValue, ignoreBlock);

        private void CalculateDamage(ref int calculatingHp, ref int calculatingPermanentBlock, ref int calculatingTemporaryBlock, int damageValue, bool ignoreBlock)
        {
            if (!ignoreBlock)
            {
                int damageToTemporaryBlock = Mathf.Clamp(damageValue, 0, calculatingTemporaryBlock);
                damageValue -= damageToTemporaryBlock;
                calculatingTemporaryBlock -= damageToTemporaryBlock;

                int damageToBlock = Mathf.Clamp(damageValue, 0, calculatingPermanentBlock);
                damageValue -= damageToBlock;
                calculatingPermanentBlock -= damageToBlock;
                
                damageValue = Mathf.Clamp(damageValue, 0, 1000);
            }

            calculatingHp -= damageValue;
        }

        public void DieFromStepOnUnit()
        {
            deathFromStepOnThisUnitAudioSource.Play();

            if (dieImmedeatlyAfterStepOnItByUnit)
            {
                _unit.AbleToDie.DieWithoutAnimation();
            }
            else
            {
                TakeDamage(maxHP, true, null);
            }
        }
        
        public void DieFromBumpInto()
        {
            deathFromStepOnThisUnitAudioSource.Play();
            _unit.AbleToDie.DieWithoutAnimation();
        }

        public void TickAfterEnemiesTurn()
        {
            if(_unit.IsEnemy)
                return;
            
            temporaryBlock = 0;
            _hashedBlock = Block;
        }

        public void TickAfterPlayerTurn()
        {
            if(!_unit.IsEnemy)
                return;
            
            temporaryBlock = 0;
            _hashedBlock = Block;
        }

        private void DieProcess()
        {
            _unit.AbleToDie.Die();
        }

        public void DisablePreVisualization()
        {
            _healthWithPreTakenDamage = currentHP;
            _permanentBlockrWithPreTakenDamage = permanentBlock;
            _temporaryBlockWithPreTakenDamage = temporaryBlock;
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        public int CalculateHashFunctionOfPrevisualisation() => 4 * MaxHp + 4 * CurrentHp + 4 * HealthWithPreTakenDamage + 4 * PermanentBlock + 4 * TemporaryBlock + 4;
    }
}