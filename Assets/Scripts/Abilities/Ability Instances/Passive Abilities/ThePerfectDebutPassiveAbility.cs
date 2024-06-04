using UnityEngine;

namespace FroguesFramework
{
    public class ThePerfectDebutPassiveAbility : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int value;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.OnCurrentRoomUpdated.AddListener(AddTemporaryActionPointsOnStart);
        }

        private void AddTemporaryActionPointsOnStart()
        {
            _owner.ActionPoints.IncreaseTemporaryPoints(value);
        }

        public int GetValue() => value;
    }
}