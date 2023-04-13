using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AbleToCostAbility : BaseAbility, IAbleToCost
    {
        [SerializeField] protected int cost;

        public virtual bool IsActionPointsEnough() => _owner.ActionPoints.IsActionPointsEnough(cost);
        public virtual int GetCost() => cost;
        public virtual void SpendActionPoints() => _owner.ActionPoints.SpendPoints(cost);
    }
}