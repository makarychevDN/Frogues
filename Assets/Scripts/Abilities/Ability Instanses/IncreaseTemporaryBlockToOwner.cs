using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseTemporaryBlockToOwner : NonTargetAbility
    {
        [SerializeField] private int blockValue;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            _owner.Health.IncreaseTemporaryArmor(blockValue);
        }
    }
}