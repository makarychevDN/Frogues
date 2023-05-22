using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AbleToUseAbility : BaseAbility, IAbleToCost
    {
        [SerializeField] protected int cost;

        [Header("Animation Setup")]
        [SerializeField] protected AbilityAnimatorTriggers abilityAnimatorTrigger;
        [SerializeField] protected WeaponIndexes weaponIndex = WeaponIndexes.NoNeedToChangeWeapon;
        [SerializeField] protected float timeBeforeImpact;
        [SerializeField] protected float fullAnimationTime;
        [SerializeField] protected float delayBeforeImpactSound;
        [SerializeField] protected AudioSource impactSoundSource;

        public virtual bool IsActionPointsEnough() => _owner.ActionPoints.IsPointsEnough(cost);
        public virtual int GetCost() => cost;
        public virtual void SpendActionPoints() => _owner.ActionPoints.SpendPoints(cost);
    }
}