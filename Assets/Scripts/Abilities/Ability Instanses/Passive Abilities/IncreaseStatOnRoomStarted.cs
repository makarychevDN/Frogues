using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatOnRoomStarted : PassiveAbility
    {
        [SerializeField] private StatEffectTypes effectType;
        [SerializeField] private int value;
        [SerializeField] private int timer;
        StatEffect _statEffect;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _statEffect = _owner.Stats.AddStatEffect(effectType, value, timer);
        }

        public override void UnInit()
        {
            _owner.Stats.RemoveStatEffect(_statEffect);
            base.UnInit();
        }
    }
}