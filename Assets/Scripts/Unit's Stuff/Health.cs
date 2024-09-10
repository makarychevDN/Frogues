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
        [SerializeField] private int thorns;
        [SerializeField] private int poison;
        [SerializeField] private int escapesFromDeath;
        [SerializeField] private bool dieImmedeatlyAfterStepOnItByUnit;
        [SerializeField] private AudioSource deathFromStepOnThisUnitAudioSource;

        [Header("Block Events")]
        public UnityEvent OnBlockIncreased;
        public UnityEvent OnDamageBlocked;
        public UnityEvent<Unit> OnDamageFromUnitBlocked;
        public UnityEvent OnBlockChargesCountUpdated;
        public UnityEvent OnBlockDestroyedByRegeneration;
        public UnityEvent OnPretakenDamageOnBlockChanged;

        [Header("Armor Events")]
        public UnityEvent OnArmorIncreased;
        public UnityEvent OnDamageReducedByArmor;
        public UnityEvent<Unit> OnDamageFromUnitReducedByArmor;

        [Header("Health Events")]
        public UnityEvent OnDamageAppledByHealth;
        public UnityEvent OnMaxHealthChanged;
        public UnityEvent<Unit> OnDamageFromUnitAppliedByHealth;
        public UnityEvent OnHpHealed;
        public UnityEvent OnPretakenDamageOnHealthChanged;

        [Header("Poison ents")]
        public UnityEvent OnPoisonValueUpdated;

        [Header("Death Events")]
        public UnityEvent OnHpEnded;
        public UnityEvent OnEscapedFromDeath;

        private int _healthWithPreTakenDamage, _armorWithPreTakenDamage, _blockWithPreTakenDamage, _escapesFromDeathWithPretakenDamage;
        private Unit _unit;

        public int MaxHp => maxHP;
        public int CurrentHp => currentHP;
        public int HealthWithPreTakenDamage => _healthWithPreTakenDamage;
        public int Armor => armor;
        public int Block => block;
        public int BlockWithPreTakenDamage => _blockWithPreTakenDamage;
        public int ArmorWithPreTakenDamage => _armorWithPreTakenDamage;
        public int EscapesFromDeath => escapesFromDeath;
        public int EscapesFromDeathCountWithPretakenDamage => _escapesFromDeathWithPretakenDamage;
        public int Thorns => thorns;
        public int Poison => poison;

        public bool Full => currentHP == maxHP;

        public void Init(Unit unit)
        {
            _unit = unit;
            OnDamageAppledByHealth.AddListener(TriggerTakeDamageAnimation);
            unit.OnStepOnThisUnit.AddListener(DieFromStepOnUnit);
            _healthWithPreTakenDamage = CurrentHp;
            _blockWithPreTakenDamage = Block;
            AddSelfToTheList();
        }

        public void IncreaseBlock(int value)
        {
            block += value;
            OnBlockIncreased.Invoke();
            OnBlockChargesCountUpdated.Invoke();
        }

        public void IncreaseArmor(int value)
        {
            armor += value;
            OnArmorIncreased.Invoke();
        }

        public void IncreaseMaxHp(int value)
        {
            maxHP += value;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            OnMaxHealthChanged.Invoke();
        }

        public void IncreaseEscapesFromDeathCount(int value)
        {
            escapesFromDeath += value;
        }

        private void TriggerTakeDamageAnimation()
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Add(this);
            _unit.Animator.SetTrigger(CharacterAnimatorParameters.TakeDamage);
            Invoke("RemoveFromCurrentlyActiveObjects", 0.25f); //todo улучшить эту штуку
        }

        private void RemoveFromCurrentlyActiveObjects()
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Remove(this);
        }

        public void TakeHealing(int value)
        {
            currentHP += value;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            OnHpHealed.Invoke();
        }

        public void TakeDamage(int damageValue, Unit damageSource) =>
            TakeDamage(damageValue, false, damageSource);

        public void TakeDamage(int damageValue, bool ignoreBlock, Unit damageSource, int newPoison = 0)
        {
            damageValue += poison;

            if(newPoison > 0)
            {
                poison += newPoison;
                OnPoisonValueUpdated.Invoke();
            }

            if(block > 0)
            {
                block--;
                OnDamageBlocked.Invoke();
                OnDamageFromUnitBlocked.Invoke(damageSource);
                OnBlockChargesCountUpdated.Invoke();
                return;
            }

            if(armor > 0)
            {
                damageValue -= armor;
                damageValue = Mathf.Clamp(damageValue, 0, 1000);
                OnDamageReducedByArmor.Invoke();
                OnDamageFromUnitReducedByArmor.Invoke(damageSource);
            }

            if(damageValue > 0)
            {
                currentHP -= damageValue;
                OnDamageAppledByHealth.Invoke();
            }

            if(currentHP <= 0)
            {
                OnHpEnded.Invoke();

                if (escapesFromDeath <= 0)
                {
                    Invoke(nameof(DieProcess), 0.25f);
                }
                else
                {
                    OnEscapedFromDeath.Invoke();
                    currentHP = maxHP / 2;
                }
            }
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

            if (preventingSystemValue > 0)
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
            CalculatePretakenDamage(ref _healthWithPreTakenDamage, ref _blockWithPreTakenDamage, ref _escapesFromDeathWithPretakenDamage,
                damageValue, armor, false);

        public void PreTakeDamage(int damageValue, bool ignoreBlock) =>
            CalculatePretakenDamage(ref _healthWithPreTakenDamage, ref _blockWithPreTakenDamage, ref _escapesFromDeathWithPretakenDamage,
                damageValue, armor, ignoreBlock);

        private void CalculatePretakenDamage(ref int calculatingHp, ref int calculatingBlock, ref int calculatingEscapeFromDeathCharges, int damageValue, int armorValue, bool ignoreBlock)
        {
            damageValue += poison;

            if (calculatingBlock > 0)
            {
                calculatingBlock--;
                OnPretakenDamageOnBlockChanged.Invoke();
                return;
            }

            damageValue -= armorValue;
            damageValue = Mathf.Clamp(damageValue, 0, 1000);

            calculatingHp -= damageValue;
            OnPretakenDamageOnHealthChanged.Invoke();

            if (calculatingHp <= 0)
                calculatingEscapeFromDeathCharges--;
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

            DecreaseBlockAfterTurnOfOwner();
        }

        public void TickAfterPlayerTurn()
        {
            if(!_unit.IsEnemy)
                return;

            DecreaseBlockAfterTurnOfOwner();
        }

        private void DecreaseBlockAfterTurnOfOwner()
        {
            int hashedBlock = block;

            block--;
            block = Mathf.Clamp(block, 0, 100);
            OnBlockChargesCountUpdated.Invoke();

            if (hashedBlock > 0)
            {
                OnBlockDestroyedByRegeneration.Invoke();
            }
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
            _escapesFromDeathWithPretakenDamage = escapesFromDeath;
            OnPretakenDamageOnHealthChanged.Invoke();
            OnPretakenDamageOnBlockChanged.Invoke();
        }

        private void OnDestroy()
        {
            RemoveSelfFromTheList();
        }

        public void AddSelfToTheList() =>
            _unit.AddAbleToDisablePrevisualizationObject(this);

        public void RemoveSelfFromTheList() =>
            _unit.RemoveAbleToDisablePrevisualizationObject(this);

        public int CalculateHashFunctionOfPrevisualisation() => 4 * MaxHp + 4 * CurrentHp + 4 * HealthWithPreTakenDamage + 4 * BlockWithPreTakenDamage + 4 * ArmorWithPreTakenDamage + 4;
    }
}