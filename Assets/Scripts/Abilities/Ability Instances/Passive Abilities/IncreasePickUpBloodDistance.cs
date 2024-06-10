using UnityEngine;

namespace FroguesFramework
{
    public class IncreasePickUpBloodDistance : PassiveAbility, IAbleToReturnRange
    {
        [SerializeField] private int distance;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.AbilitiesManager.Abilities.ForEach(ability => TryToIncreaseDistanceForAdaptation(ability));

            _owner.AbilitiesManager.OnAbilityHasBeenAdded.AddListener(TryToIncreaseDistanceForAdaptation);
            _owner.AbilitiesManager.OnAbilityHasBeenRemoved.AddListener(TryToDencreaseDistanceForAdaptation);
        }

        public override void UnInit()
        {
            _owner.AbilitiesManager.OnAbilityHasBeenAdded.RemoveListener(TryToIncreaseDistanceForAdaptation);
            _owner.AbilitiesManager.OnAbilityHasBeenRemoved.RemoveListener(TryToDencreaseDistanceForAdaptation);

            _owner.AbilitiesManager.Abilities.ForEach(ability => TryToDencreaseDistanceForAdaptation(ability));

            base.UnInit();
        }

        public int ReturnRange() => distance;

        private void TryToIncreaseDistanceForAdaptation(BaseAbility baseAbility)
        {
            var adaptation = baseAbility as AdaptationPassiveAbility;
            if (adaptation == null)
                return;

            adaptation.AdditionalDistance += distance;
        }

        private void TryToDencreaseDistanceForAdaptation(BaseAbility baseAbility)
        {
            var adaptation = baseAbility as AdaptationPassiveAbility;
            if (adaptation == null)
                return;

            adaptation.AdditionalDistance -= distance;
        }
    }
}