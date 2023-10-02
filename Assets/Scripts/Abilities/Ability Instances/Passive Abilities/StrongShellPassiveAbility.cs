using System;
using UnityEngine;

namespace FroguesFramework
{
    public class StrongShellPassiveAbility : PassiveAbility
    {
        [SerializeField] private int additionalConstantlyBlockValue;
        [SerializeField] private int healthCostValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseMaxHp(-healthCostValue);
            EntryPoint.Instance.OnNextRoomStarted.AddListener(AddConstantlyBlock);
        }

        public override void UnInit()
        {
            _owner.Health.IncreaseMaxHp(healthCostValue);
            EntryPoint.Instance.OnNextRoomStarted.RemoveListener(AddConstantlyBlock);
            base.UnInit();
        }

        private void AddConstantlyBlock()
        {
            _owner.Health.IncreasePermanentBlock(additionalConstantlyBlockValue);
        }
    }
}