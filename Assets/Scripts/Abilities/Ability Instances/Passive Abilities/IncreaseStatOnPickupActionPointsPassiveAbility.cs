using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatOnPickupActionPointsPassiveAbility : PassiveAbility, IAbleToHaveCount
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
            _owner.OnCurrentRoomUpdated.AddListener(ResetCounter);
            ResetCounter();
        }

        public override void UnInit()
        {
            _owner.ActionPoints.OnPickUpPoints.RemoveListener(TryToIncreaseStat);
            _owner.OnCurrentRoomUpdated.RemoveListener(ResetCounter);
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

        public int GetCount() => requiredPickupPointsInstancesToBuff;
    }
}