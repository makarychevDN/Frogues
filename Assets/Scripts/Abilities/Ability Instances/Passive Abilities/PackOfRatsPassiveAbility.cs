using UnityEngine;

namespace FroguesFramework
{
    public class PackOfRatsPassiveAbility : PassiveAbility, IAbleToReturnSingleValue, IAbleToHaveCount, IAbleToApplyStrenghtModificator
    {
        [SerializeField] private int additionalStrenghtForEachRat;
        [SerializeField] private StatEffect effectSetup;
        private StatEffect _effect;

        public int GetCount() => additionalStrenghtForEachRat * (_owner.CurrentRoom.RatsInTheRoomCount - 1);
        public int GetDeltaOfStrenghtValueForEachTurn() => effectSetup.deltaValueForEachTurn;
        public bool GetStrenghtEffectIsConstantly() => effectSetup.effectIsConstantly;
        public int GetStrenghtModificatorValue() => effectSetup.Value;
        public int GetTimeToEndOfStrenghtEffect() => effectSetup.timeToTheEndOfEffect;
        public int GetValue() => additionalStrenghtForEachRat;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _effect = new StatEffect(effectSetup);
            _owner.Stats.AddStatEffect(_effect);
            _owner.CurrentRoom.OnCountOfRatsUpdated.AddListener(UpdateEffectValue);
            _owner.AbleToDie.OnDeath.AddListener(DecreaseCountOfRats);
        }

        public override void UnInit()
        {
            _owner.Stats.RemoveStatEffect(_effect);
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
            _effect.Value = (newValue - 2) * additionalStrenghtForEachRat;
        }
    }
}