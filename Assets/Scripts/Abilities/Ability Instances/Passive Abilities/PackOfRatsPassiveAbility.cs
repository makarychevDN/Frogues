using UnityEngine;

namespace FroguesFramework
{
    public class PackOfRatsPassiveAbility : PassiveAbility, IAbleToReturnSingleValue, IAbleToHaveCount
    {
        [SerializeField] private int additionalStrenghtForEachRat;
        [SerializeField] private StatEffect _effect;

        public int GetCount() => additionalStrenghtForEachRat * (EntryPoint.Instance.CountOfRats - 1);

        public int GetValue() => additionalStrenghtForEachRat;

        public override void Init(Unit unit)
        {
            base.Init(unit);

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
            _effect.Value = (newValue - 1);
        }
    }
}