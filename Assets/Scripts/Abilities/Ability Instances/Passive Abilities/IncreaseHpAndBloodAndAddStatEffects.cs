using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseHpAndBloodAndAddStatEffects : PassiveAbility
    {
        [SerializeField] private int additionalHp;
        [SerializeField] private int additionalMaxBlood;
        [SerializeField] private List<StatEffect> effects;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseMaxHp(additionalHp);
            _owner.BloodPoints.IncreaseLimit(additionalMaxBlood);
            effects.ForEach(effect => _owner.Stats.AddStatEffect(effect));
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Health.IncreaseMaxHp(-additionalHp);
            _owner.BloodPoints.IncreaseLimit(-additionalMaxBlood);
            effects.ForEach(effect => _owner.Stats.RemoveStatEffect(effect));
        }
    }
}   