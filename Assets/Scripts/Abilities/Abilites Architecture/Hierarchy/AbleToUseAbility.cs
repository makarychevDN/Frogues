using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AbleToUseAbility : BaseAbility, IAbleToCost, IAbleToHaveCooldown, IRoundTickable
    {
        [SerializeField] protected int actionPointsCost;
        [SerializeField] protected int bloodPointsCost;
        [SerializeField] protected int cooldownAfterUse;
        [SerializeField] protected int cooldownAfterStart;
        [SerializeField] protected int cooldownCounter;
        [SerializeField] protected bool isCooldownedAfterStart;

        [Header("Animation Setup")]
        [SerializeField] protected AbilityAnimatorTriggers abilityAnimatorTrigger;
        [SerializeField] protected WeaponIndexes weaponIndex = WeaponIndexes.NoNeedToChangeWeapon;
        [SerializeField] protected float timeBeforeImpact;
        [SerializeField] protected float fullAnimationTime;
        [SerializeField] protected float delayBeforeImpactSound;
        [SerializeField] protected AudioSource impactSoundSource;

        public virtual int GetBloodPointsCost() => bloodPointsCost;
        public int GetActionPointsCost() => actionPointsCost;

        public virtual void SpendResourcePoints()
        {
            _owner.ActionPoints.SpendPoints(actionPointsCost);

            if (_owner.BloodPoints != null)
                _owner.BloodPoints.SpendPoints(bloodPointsCost);
        }

        public virtual bool IsResoursePointsEnough()
        {
            if (_owner.BloodPoints != null)
            {
                return _owner.ActionPoints.IsPointsEnough(actionPointsCost)
                    && _owner.BloodPoints.IsPointsEnough(bloodPointsCost);
            }

            return _owner.ActionPoints.IsPointsEnough(actionPointsCost);
        }

        #region cooldownsStuff

        public void DecreaseCooldown(int value = 1)
        {
            cooldownCounter--;
            cooldownCounter = Mathf.Clamp(cooldownCounter, 0, 99);

            if(cooldownCounter == 0)
                isCooldownedAfterStart = true;
        }

        public void SetCooldownAsAfterStart()
        {
            cooldownCounter = cooldownAfterStart;
            isCooldownedAfterStart = false;
        }   

        public void SetCooldownAsAfterUse() => cooldownCounter = cooldownAfterUse;

        public bool IsCooldowned() => cooldownCounter == 0;

        public void TickBeforePlayerTurn()
        {
            if (_owner == null)
                return;

            if (!_owner.IsEnemy)
                DecreaseCooldown();
        }

        public void TickBeforeEnemiesTurn()
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

        #endregion
    }
}