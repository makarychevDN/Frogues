using UnityEngine;

namespace FroguesFramework
{
    public class AddBlockOnTheEndOfTurnPassiveAbility : PassiveAbility
    {
        [SerializeField] private int blockValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(IncreaseBlock);
        }

        private void IncreaseBlock()
        {
            _owner.Health.IncreaseTemporaryBlock(blockValue);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(IncreaseBlock);
        }
    }
}
