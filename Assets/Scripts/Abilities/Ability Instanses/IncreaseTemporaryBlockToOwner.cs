using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseTemporaryBlockToOwner : NonTargetAbility
    {
        [SerializeField] private int blockValue;

        public override void Use()
        {
            _owner.Health.IncreaseTemporaryArmor(blockValue);
        }
    }
}