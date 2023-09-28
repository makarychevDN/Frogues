using UnityEngine;

namespace FroguesFramework
{
    public class ThePerfectDebutPassiveAbility : PassiveAbility
    {
        [SerializeField] private int value;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            EntryPoint.Instance.OnNextRoomStarted.AddListener(AddTemporaryActionPointsOnStart);
        }

        private void AddTemporaryActionPointsOnStart()
        {
            _owner.ActionPoints.IncreaseTemporaryPoints(value);
        }
    }
}