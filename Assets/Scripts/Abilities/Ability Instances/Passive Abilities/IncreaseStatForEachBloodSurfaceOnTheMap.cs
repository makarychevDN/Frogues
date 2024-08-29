using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatForEachBloodSurfaceOnTheMap : PassiveAbility, IAbleToApplyAnyModificator, IAbleToHaveCount
    {
        [SerializeField] private int requredCountOfBloodToIncreaseStat;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.CurrentRoom.OnCountOfBloodPuddlesUpdated.AddListener(UpdateStatEffect);
            UpdateStatEffect(_owner.CurrentRoom.BloodPuddlesInTheRoomCount);
        }

        public override void UnInit()
        {
            _owner.CurrentRoom.OnCountOfBloodPuddlesUpdated.RemoveListener(UpdateStatEffect);
            base.UnInit();
        }

        private void UpdateStatEffect(int countOfBloodPuddles)
        {
        }

        public int GetModificatorValue() => 1;

        public int GetDeltaValueForEachTurn() => 0;

        public int GetTimeToEndOfEffect() => 0;

        public bool GetEffectIsConstantly() => true;

        public int GetCount() => requredCountOfBloodToIncreaseStat;
    }
}