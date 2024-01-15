using UnityEngine;

namespace FroguesFramework
{
    public class SpawnSurfaceOnDeathPassiveAbility : PassiveAbility
    {
        [SerializeField] private Unit surfacePrefab;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToDie.OnDeath.AddListener(SpawnSurfaceUnderOwner);
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.AbleToDie.OnDeath.RemoveListener(SpawnSurfaceUnderOwner);
        }

        private void SpawnSurfaceUnderOwner()
        {
            EntryPoint.Instance.SpawnUnit(surfacePrefab, _owner.CurrentCell);
        }
    }
}