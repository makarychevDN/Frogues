using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatForEachBloodSurfaceOnTheMap : PassiveAbility, IAbleToApplyAnyModificator, IAbleToHaveCount
    {
        [SerializeField] private StatEffectTypes type;
        [SerializeField] private int requredCountOfBloodToIncreaseStat;
        private StatEffect _statEffect;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _statEffect = new StatEffect(type, 0, 1, 0, true);
            _owner.Stats.AddStatEffect(_statEffect);
            _owner.CurrentRoom.OnCountOfBloodPuddlesUpdated.AddListener(UpdateStatEffect);
            UpdateStatEffect(_owner.CurrentRoom.BloodPuddlesInTheRoomCount);
        }

        public override void UnInit()
        {
            _owner.CurrentRoom.OnCountOfBloodPuddlesUpdated.RemoveListener(UpdateStatEffect);
            _owner.Stats.RemoveStatEffect(_statEffect);
            base.UnInit();
        }

        private void UpdateStatEffect(int countOfBloodPuddles)
        {
            _statEffect.Value = countOfBloodPuddles / requredCountOfBloodToIncreaseStat;
        }

        public int GetModificatorValue() => 1;

        public int GetDeltaValueForEachTurn() => 0;

        public int GetTimeToEndOfEffect() => 0;

        public bool GetEffectIsConstantly() => true;

        public int GetCount() => requredCountOfBloodToIncreaseStat;
    }
}