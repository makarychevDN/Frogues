using UnityEngine;

namespace FroguesFramework
{
    public abstract class AbleToUseAbility : BaseAbility, IAbleToCost, IAbleToHaveCooldown, IRoundTickable
    {
        [SerializeField] protected int actionPointsCost;
        [SerializeField] protected int bloodPointsCost;
        [SerializeField] protected int healthCost;

        [Header("Cooldowns setup")]
        [SerializeField] protected int cooldownAfterUse;
        [SerializeField] protected int cooldownAfterStart;
        [SerializeField] protected int cooldownCounter;
        [SerializeField] protected bool isCooldownedAfterStart;

        [Header("Charges setup")]
        [SerializeField] protected int maxChargesCount = 1;
        [SerializeField] protected int chargesCountAfterStart = 0;
        [SerializeField] protected int costOfEachUsingInCharges = 1;
        [SerializeField] protected int currentCharges = 1;

        [Header("Animation Setup")]
        [SerializeField] protected AbilityAnimatorTriggers abilityAnimatorTrigger;
        [SerializeField] protected RuntimeAnimatorController animatorControllerForAbility;
        [SerializeField] protected float timeBeforeImpact = 0.15f;
        [SerializeField] protected float fullAnimationTime = 0.8f;
        [SerializeField] protected float delayBeforeImpactSound;
        [SerializeField] protected AudioSource impactSoundSource;

        public virtual int GetBloodPointsCost() => bloodPointsCost;
        public virtual int GetActionPointsCost() => actionPointsCost;
        public virtual int GetHealthCost() => healthCost;
        public void IncreaseActionPointsCost(int value) => actionPointsCost += value;
        public void IncreaseBloodPointsCost(int value) => bloodPointsCost += value;
        public void IncreaseHealthCost(int value) => healthCost += value;

        public virtual void SpendResourcePoints()
        {
            _owner.ActionPoints.SpendPoints(CalculateActionPointsCost);

            if (_owner.BloodPoints != null)
                _owner.BloodPoints.SpendPoints(CalculateBloodPointsCost);

            _owner.Health.TakeDamage(healthCost, true, null);

            currentCharges -= costOfEachUsingInCharges;
        }

        public virtual bool IsResoursePointsEnough()
        {
            if (_owner.BloodPoints != null)
            {
                return _owner.ActionPoints.IsPointsEnough(CalculateActionPointsCost)
                    && _owner.BloodPoints.IsPointsEnough(CalculateBloodPointsCost);
            }

            return _owner.ActionPoints.IsPointsEnough(actionPointsCost);
        }

        protected virtual int CalculateActionPointsCost => actionPointsCost;

        protected virtual int CalculateBloodPointsCost => bloodPointsCost;

        #region cooldownsStuff

        public void SpendCharges()
        {
            currentCharges -= costOfEachUsingInCharges;
        }


        public void DecreaseCooldown(int value = 1)
        {
            cooldownCounter--;
            cooldownCounter = Mathf.Clamp(cooldownCounter, 0, 99);

            if(cooldownCounter == 0)
            {
                isCooldownedAfterStart = true;

                if(currentCharges < maxChargesCount)
                {
                    currentCharges++;
                    cooldownCounter = cooldownAfterUse;

                    if (currentCharges == maxChargesCount)
                        cooldownCounter = 0;
                }
            }

        }

        public void SetCooldownAsAfterStart()
        {
            cooldownCounter = cooldownAfterStart;
            isCooldownedAfterStart = false;
        }

        public void SetCooldownAsAfterUse() 
        { 
            if (cooldownCounter == 0) 
                cooldownCounter = cooldownAfterUse; 
        }

        public bool IsEnoughCharges() => currentCharges >= costOfEachUsingInCharges;

        public virtual void TickAfterEnemiesTurn()
        {
            if (_owner == null)
                return;

            if (!_owner.IsEnemy)
                DecreaseCooldown();
        }

        public virtual void TickAfterPlayerTurn()
        {
            if (_owner == null)
                return;

            if (_owner.IsEnemy)
                DecreaseCooldown();
        }

        public int GetCooldownCounter() => cooldownCounter;

        public int GetCooldownAfterStart() => cooldownAfterStart;

        public int GetCooldownAfterUse() => cooldownAfterUse;

        public bool GetCooldownAfterStartIsDone() => isCooldownedAfterStart;

        public int GetCurrentCooldown() => GetCooldownAfterStartIsDone() ? GetCooldownAfterUse() : GetCooldownAfterStart();

        public int GetCurrentCharges() => currentCharges;

        #endregion
    }
}