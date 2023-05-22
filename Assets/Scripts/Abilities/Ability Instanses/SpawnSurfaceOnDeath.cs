using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnSurfaceOnDeath : PassiveAbility
    {
        [SerializeField] private Unit surfacePrefab;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.AbleToDie.OnDeath.AddListener(SpawnSurfaceUnderOwner);
        }

        private void SpawnSurfaceUnderOwner()
        {
            //var currentBloodSurface = 
                //_owner.CurrentCell.Surfaces.First(surface => surface.GetComponentInChildren<SpawnSurfaceOnDeath>());

            //if (currentBloodSurface == null)
                //return;

            var surfaceInstance = Instantiate(surfacePrefab);
            surfaceInstance.CurrentCell = _owner.CurrentCell;
            surfaceInstance.Init();
            _owner.CurrentCell.Surfaces.Add(surfaceInstance);
            surfaceInstance.transform.position = _owner.CurrentCell.transform.position;
        }
    }
}