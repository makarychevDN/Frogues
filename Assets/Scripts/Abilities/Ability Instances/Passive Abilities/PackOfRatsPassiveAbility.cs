using UnityEngine;

namespace FroguesFramework
{
    public class PackOfRatsPassiveAbility : PassiveAbility, IAbleToReturnSingleValue, IAbleToHaveCount, IAbleToApplyStrenghtModificator
    {
        [SerializeField] private int additionalStrenghtForEachRat;
        [SerializeField] private StatEffect effectSetup;
        private StatEffect _effect;

        public int GetCount() => additionalStrenghtForEachRat * (EntryPoint.Instance.CountOfRats - 1);

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
            EntryPoint.Instance.OnCountOfRatsUpdated.AddListener(UpdateEffectValue);
            EntryPoint.Instance.CountOfRats++;
            _owner.AbleToDie.OnDeath.AddListener(DecreaseCountOfRats);
        }

        public override void UnInit()
        {
            _owner.Stats.RemoveStatEffect(_effect);
            DecreaseCountOfRats();
            EntryPoint.Instance.OnCountOfRatsUpdated.RemoveListener(UpdateEffectValue);
            _owner.AbleToDie.OnDeath.RemoveListener(DecreaseCountOfRats);

            base.UnInit();
        }

        private void DecreaseCountOfRats()
        {
            EntryPoint.Instance.CountOfRats--;
        }

        private void UpdateEffectValue(int newValue)
        {
            _effect.Value = (newValue - 2) * additionalStrenghtForEachRat;
        }
    }
}