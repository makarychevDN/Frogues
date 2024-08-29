using UnityEngine;

namespace FroguesFramework
{
    public class PackOfRatsPassiveAbility : PassiveAbility, IAbleToReturnSingleValue, IAbleToHaveCount
    {
        [SerializeField] private int additionalStrenghtForEachRat;

        public int GetCount() => additionalStrenghtForEachRat * (_owner.CurrentRoom.RatsInTheRoomCount - 1);
        public int GetValue() => additionalStrenghtForEachRat;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.CurrentRoom.OnCountOfRatsUpdated.AddListener(UpdateEffectValue);
            _owner.AbleToDie.OnDeath.AddListener(DecreaseCountOfRats);
        }

        public override void UnInit()
        {
            DecreaseCountOfRats();
            _owner.CurrentRoom.OnCountOfRatsUpdated.RemoveListener(UpdateEffectValue);
            _owner.AbleToDie.OnDeath.RemoveListener(DecreaseCountOfRats);

            base.UnInit();
        }

        private void DecreaseCountOfRats()
        {
            _owner.CurrentRoom.RatsInTheRoomCount--;
        }

        private void UpdateEffectValue(int newValue)
        {
        }
    }
}