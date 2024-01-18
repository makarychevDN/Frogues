using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatOnPickupBloodPointsPassiveAbility : PassiveAbility, IAbleToApplyIntelligenceModificator
    {
        [SerializeField] private int valueOfEffect = 1;
        [SerializeField] private int timerOfEffect = 1;
        [SerializeField] private StatEffectTypes type;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.BloodPoints.OnPickUpPoints.AddListener(IncreaseStat);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.BloodPoints.OnPickUpPoints.RemoveListener(IncreaseStat);
        }

        private void IncreaseStat()
        {
            _owner.Stats.AddStatEffect(new StatEffect(type, valueOfEffect, timerOfEffect));
        }

        public int GetDeltaOfIntelligenceValueForEachTurn() => 0;

        public bool GetIntelligenceEffectIsConstantly() => false;

        public int GetIntelligenceModificatorValue() => valueOfEffect;

        public int GetTimeToEndOfIntelligenceEffect() => timerOfEffect;
    }
}