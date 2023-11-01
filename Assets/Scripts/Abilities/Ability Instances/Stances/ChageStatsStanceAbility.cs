using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ChageStatsStanceAbility : BattleStanceAbility
    {
        [SerializeField] private List<StatEffect> effects;

        public override void ApplyEffect(bool isActive)
        {
            base.ApplyEffect(isActive);

            if (isActive)
            {
                effects.ForEach(effect => _owner.Stats.AddStatEffect(effect));
            }
            else
            {
                effects.ForEach(effect => _owner.Stats.RemoveStatEffect(effect));
            }
        }
    }
}