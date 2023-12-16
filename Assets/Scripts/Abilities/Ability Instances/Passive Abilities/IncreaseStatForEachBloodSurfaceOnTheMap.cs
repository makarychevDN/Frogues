using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatForEachBloodSurfaceOnTheMap : PassiveAbility
    {
        [SerializeField] private StatEffectTypes type;
        private StatEffect _statEffect;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _statEffect = new StatEffect(type, CalculateValue(), 1, 0, true);
            _owner.Stats.AddStatEffect(_statEffect);
            EntryPoint.Instance.OnBloodSurfacesCountOnTheMapUpdated.AddListener(UpdateStatEffect);
        }

        public override void UnInit()
        {
            EntryPoint.Instance.OnBloodSurfacesCountOnTheMapUpdated.RemoveListener(UpdateStatEffect);
            _owner.Stats.RemoveStatEffect(_statEffect);
            base.UnInit();
        }

        private void UpdateStatEffect()
        {
            _statEffect.Value = CalculateValue() / 2;
        }

        private int CalculateValue()
        {
            return EntryPoint.Instance.BloodSurfacesCount;
        }
    }
}