using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Health : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization, IAbleToCalculateHashFunctionOfPrevisualisation
    {
        [SerializeField] private int maxHP;
        [SerializeField] private int currentHP;
        [SerializeField] private int armor;
        [SerializeField] private int block;
        [SerializeField] private int escapesFromDeathCount;
        [SerializeField] private bool dieImmedeatlyAfterStepOnItByUnit;
        [SerializeField] private AudioSource deathFromStepOnThisUnitAudioSource;

        [Header("Increase Parameters Events")]
        public UnityEvent OnBlockIncreased;
        public UnityEvent OnArmorIncreased;
        public UnityEvent OnArmorOrBlockIncreased;
        public UnityEvent OnHpHealed;

        [Header("Apply Damage On Block Events")]
        public UnityEvent OnDamageAppliedByBlock;
        public UnityEvent<Unit> OnDamageFromUnitAppliedByBlock;
        public UnityEvent OnDamagePreventedByBlock;
        public UnityEvent<Unit> OnDamageFromUnitPreventedByBlock;
        public UnityEvent OnBlockDestroyed;
        public UnityEvent<Unit> OnBlockDestroyedByUnit;

        [Header("Apply Damage On Armor Events")]
        public UnityEvent OnDamageAppliedByArmor;
        public UnityEvent<Unit> OnDamageFromUnitAppliedByArmor;
        public UnityEvent OnDamagePreventedByArmor;
        public UnityEvent<Unit> OnDamageFromUnitPreventedByArmor;
        public UnityEvent OnArmorDestroyed;
        public UnityEvent<Unit> OnArmorDestroyedByUnit;

        [Header("Apply Damage On Health Events")]
        public UnityEvent OnDamageAppledByHealth;
        public UnityEvent<Unit> OnDamageFromUnitAppliedByHealth;

        [Header("Death Events")]
        public UnityEvent OnHpEnded;
        public UnityEvent OnEscapedFromDeath;
        
        private int _healthWithPreTakenDamage, _armorWithPreTakenDamage, _blockWithPreTakenDamage;
        private int _hashedHp, _hashedBlock, _hashedArmor;
        private Unit _unit;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;
        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;
        public int Armor => armor;
        public int Block => block;
        public int BlockWithPreTakenDamage => _blockWithPreTakenDamage;
        public int ArmorWithPreTakenDamage => _armorWithPreTakenDamage;

        public bool Full => currentHP == maxHP;

        public void Init(Unit unit)
        {
            _unit = unit;
            _hashedHp = currentHP;
            _hashedArmor = armor;
            _hashedBlock = block;
            OnDamageAppledByHealth.AddListener(TriggerTakeDamageAnimation);
            unit.OnStepOnThisUnit.AddListener(DieFromStepOnUnit);
            AddMySelfToEntryPoint();
        }

        public void RemoveAllBlockEffects()
        {
            block = 0;
            armor = 0;
        }

        public void IncreaseBlock(int value)
        {
            block += (int)(value * _unit.Stats.DefenceModificator);
            _hashedBlock = block;
            OnBlockIncreased.Invoke();
            OnArmorOrBlockIncreased.Invoke();
        }

        public void IncreaseArmor(int value)
        {
            armor += (int)(value * _unit.Stats.DefenceModificator);
            _hashedArmor = armor;
            OnArmorIncreased.Invoke();
            OnArmorOrBlockIncreased.Invoke();
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
            _hashedHp = currentHP;
            _hashedArmor = armor;
            _hashedBlock = block;
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
            CalculateDamage(ref currentHP, ref armor, ref block, damageValue, ignoreBlock);

            if (!ignoreBlock)
            {
                if (_hashedBlock != 0)
                {
                    OnDamageApplyedByAnyPreventingSystem(block, _hashedBlock, damageSource,
                        OnDamageAppliedByBlock, OnDamageFromUnitAppliedByBlock,
                        OnDamagePreventedByBlock, OnDamageFromUnitPreventedByBlock,
                        OnBlockDestroyed, OnBlockDestroyedByUnit);
                }

                if (_hashedArmor != 0)
                {
                    OnDamageApplyedByAnyPreventingSystem(armor, _hashedArmor, damageSource,
                        OnDamageAppliedByArmor, OnDamageFromUnitAppliedByArmor,
                        OnDamagePreventedByArmor, OnDamageFromUnitPreventedByArmor,
                        OnArmorDestroyed, OnArmorDestroyedByUnit);
                }
            }

            if(damageSource != null && _unit.Stats.Spikes > 0)
            {
                damageSource.Health.TakeDamage(_unit.Stats.Spikes, null);
            }

            if (currentHP < _hashedHp)
            {
                CurrentlyActiveObjects.Add(this);
                OnDamageAppledByHealth.Invoke();
                OnDamageFromUnitAppliedByHealth.Invoke(damageSource);
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
            _hashedArmor = armor;
            _hashedBlock = block;
        }

        private void OnDamageApplyedByAnyPreventingSystem(int preventingSystemValue, int hashedPreventingSystemValue, Unit damageSource,
            UnityEvent OnDamageApplyed, UnityEvent<Unit> OnDamageFromUnitApplyed, 
            UnityEvent OnDamagePrevented, UnityEvent<Unit> OnDamageFromUnitPrevented, 
            UnityEvent OnPreventingSystemDestroyed, UnityEvent<Unit> OnPreventingSystemDestroyedByUnit)
        {
            if (preventingSystemValue == hashedPreventingSystemValue)
                return;

            OnDamageApplyed.Invoke();
            OnDamageFromUnitApplyed.Invoke(damageSource);

            if (preventingSystemValue != 0)
            {
                OnDamagePrevented.Invoke();
                OnDamageFromUnitPrevented.Invoke(damageSource);
            }
            else
            {
                OnPreventingSystemDestroyed.Invoke();
                OnPreventingSystemDestroyedByUnit.Invoke(damageSource);
            }
        }

        public void PreTakeDamage(int damageValue) =>
            CalculateDamage(ref _healthWithPreTakenDamage, ref _armorWithPreTakenDamage,
                ref _blockWithPreTakenDamage, damageValue, false);

        public void PreTakeDamage(int damageValue, bool ignoreBlock) =>
            CalculateDamage(ref _healthWithPreTakenDamage, ref _armorWithPreTakenDamage,
                ref _blockWithPreTakenDamage, damageValue, ignoreBlock);

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
            
            block = 0;
            _hashedBlock = block;
        }

        public void TickAfterPlayerTurn()
        {
            if(!_unit.IsEnemy)
                return;
            
            block = 0;
            _hashedBlock = block;
        }

        private void DieProcess()
        {
            _unit.AbleToDie.Die();
        }

        public void DisablePreVisualization()
        {
            _healthWithPreTakenDamage = currentHP;
            _armorWithPreTakenDamage = armor;
            _blockWithPreTakenDamage = block;
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        public int CalculateHashFunctionOfPrevisualisation() => 4 * MaxHp + 4 * CurrentHp + 4 * HealthWithPreTakenDamage + 4 * BlockWithPreTakenDamage + 4 * ArmorWithPreTakenDamage + 4;
    }
}