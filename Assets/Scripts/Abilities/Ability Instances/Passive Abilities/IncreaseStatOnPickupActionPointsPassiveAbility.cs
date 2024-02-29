using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatOnPickupActionPointsPassiveAbility : PassiveAbility, IAbleToApplyIntelligenceModificator, IAbleToHaveCount
    {
        [SerializeField] private int valueOfEffect = 1;
        [SerializeField] private int timerOfEffect = 1;
        [SerializeField] private int requiredPickupPointsInstancesToBuff = 2;
        [SerializeField] private StatEffectTypes type;
        private int _counter;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.ActionPoints.OnPickUpPoints.AddListener(TryToIncreaseStat);
            EntryPoint.Instance.OnNextRoomStarted.AddListener(ResetCounter);
            ResetCounter();
        }

        public override void UnInit()
        {
            _owner.ActionPoints.OnPickUpPoints.RemoveListener(TryToIncreaseStat);
            EntryPoint.Instance.OnNextRoomStarted.RemoveListener(ResetCounter);
            base.UnInit();
        }

        private void TryToIncreaseStat()
        {
            _counter++;

            if(_counter >= requiredPickupPointsInstancesToBuff)
            {
                _owner.Stats.AddStatEffect(new StatEffect(type, valueOfEffect, timerOfEffect));
                ResetCounter();
            }
        }

        private void ResetCounter() => _counter = 0;

        public int GetDeltaOfIntelligenceValueForEachTurn() => 0;

        public bool GetIntelligenceEffectIsConstantly() => false;

        public int GetIntelligenceModificatorValue() => valueOfEffect;

        public int GetTimeToEndOfIntelligenceEffect() => timerOfEffect;

        public int GetCount() => requiredPickupPointsInstancesToBuff;
    }
}