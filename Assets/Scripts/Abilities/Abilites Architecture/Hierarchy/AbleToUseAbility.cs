using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AbleToUseAbility : BaseAbility, IAbleToCost
    {
        [SerializeField] protected int actionPointsCost;
        [SerializeField] protected int bloodPointsCost;

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

            if(_owner.BloodPoints != null)
                _owner.BloodPoints.SpendPoints(bloodPointsCost);
        }

        public virtual bool IsResoursePointsEnough()
        {
            if(_owner.BloodPoints != null)
            {
                return _owner.ActionPoints.IsPointsEnough(actionPointsCost)
                    && _owner.BloodPoints.IsPointsEnough(bloodPointsCost);
            }

            return _owner.ActionPoints.IsPointsEnough(actionPointsCost);
        }
    }
}