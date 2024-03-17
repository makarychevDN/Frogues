using UnityEngine;

namespace FroguesFramework
{
    public class IncreaceStatForMissingHealthPassiveAbility : PassiveAbility, IAbleToHaveDelta
    {
        [SerializeField] private StatEffectTypes statEffectType;
        [SerializeField] private int missingHealthStep;
        [SerializeField] private int additionalStrenghtForEachStep;
        [SerializeField] private int additionalStrenghtValue;
        private StatEffect statEffect;

        public int GetDeltaValue() => additionalStrenghtForEachStep;

        public int GetStepValue() => missingHealthStep;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            statEffect = new StatEffect(statEffectType, additionalStrenghtValue, 0, 0, true);
            _owner.Stats.AddStatEffect(statEffect);
            _owner.Health.OnDamageAppledByHealth.AddListener(RecalculateStrenght);
            _owner.Health.OnHpHealed.AddListener(RecalculateStrenght);
            RecalculateStrenght();
        }

        public override void UnInit()
        {
            _owner.Stats.RemoveStatEffect(statEffect);
            _owner.Health.OnDamageAppledByHealth.RemoveListener(RecalculateStrenght);
            _owner.Health.OnHpHealed.RemoveListener(RecalculateStrenght);
            base.UnInit();
        }

        private void RecalculateStrenght()
        {
            additionalStrenghtValue = (_owner.Health.MaxHp - _owner.Health.CurrentHp) / missingHealthStep * additionalStrenghtForEachStep;
            statEffect.Value = additionalStrenghtValue;
        }
    }
}