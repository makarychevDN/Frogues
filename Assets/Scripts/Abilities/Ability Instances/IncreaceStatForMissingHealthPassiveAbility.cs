using UnityEngine;

namespace FroguesFramework
{
    public class IncreaceStatForMissingHealthPassiveAbility : PassiveAbility, IAbleToHaveDelta
    {
        [SerializeField] private int missingHealthStep;
        [SerializeField] private int additionalStrenghtForEachStep;
        [SerializeField] private int additionalStrenghtValue;

        public int GetDeltaValue() => additionalStrenghtForEachStep;

        public int GetStepValue() => missingHealthStep;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.OnDamageAppledByHealth.AddListener(RecalculateStrenght);
            _owner.Health.OnHpHealed.AddListener(RecalculateStrenght);
            RecalculateStrenght();
        }

        public override void UnInit()
        {
            _owner.Health.OnDamageAppledByHealth.RemoveListener(RecalculateStrenght);
            _owner.Health.OnHpHealed.RemoveListener(RecalculateStrenght);
            base.UnInit();
        }

        private void RecalculateStrenght()
        {
            additionalStrenghtValue = (_owner.Health.MaxHp - _owner.Health.CurrentHp) / missingHealthStep * additionalStrenghtForEachStep;
        }
    }
}