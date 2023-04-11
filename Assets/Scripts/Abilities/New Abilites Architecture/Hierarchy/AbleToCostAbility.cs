using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AbleToCostAbility : BaseAbility, IAbleToCost
    {
        [SerializeField] protected int cost;

        public bool IsActionPointsEnough() => _owner.ActionPoints.IsActionPointsEnough(cost);
        public int GetCost() => cost;
    }
}