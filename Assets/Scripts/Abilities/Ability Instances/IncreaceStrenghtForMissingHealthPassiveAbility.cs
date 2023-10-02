using UnityEngine;

namespace FroguesFramework
{
    public class IncreaceStrenghtForMissingHealthPassiveAbility : PassiveAbility
    {
        [SerializeField] private int missingHealthStep;
        [SerializeField] private int additionalStrenghtForEachStep;
        [SerializeField] private int additionalStrenghtValue;
        private StatEffect statEffect;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            statEffect = new StatEffect(StatEffectTypes.strenght, additionalStrenghtValue, 1, true);
            _owner.Stats.AddStatEffect(statEffect);
            _owner.Health.OnApplyUnblockedDamage.AddListener(RecalculateStrenght);
            _owner.Health.OnHpHealed.AddListener(RecalculateStrenght);
            RecalculateStrenght();
        }

        public override void UnInit()
        {
            _owner.Stats.RemoveStatEffect(statEffect);
            _owner.Health.OnApplyUnblockedDamage.RemoveListener(RecalculateStrenght);
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