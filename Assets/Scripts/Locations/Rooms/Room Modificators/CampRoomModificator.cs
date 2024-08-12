using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CampRoomModificator : RoomModificator
    {
        [SerializeField] private List<WeaponAbilitiesSetter> weaponSetterPrefabs;
        [SerializeField] private List<Cell> weaponSettersCells;

        public override void Init(Room room)
        {
            foreach(var cell in weaponSettersCells)
            {
                var randomWeaponSetterPrefab = weaponSetterPrefabs.GetRandomElement();
                SpawnAndInitWeaponSetterPrefab(randomWeaponSetterPrefab, cell);
                weaponSetterPrefabs.Remove(randomWeaponSetterPrefab);
            }
        }

        private void SpawnAndInitWeaponSetterPrefab(WeaponAbilitiesSetter weaponSetterPrefab, Cell cell)
        {
            var weaponSetter = Instantiate(weaponSetterPrefab, transform);
            weaponSetter.transform.localPosition = Vector3.zero;
            weaponSetter.Init(cell);
        }
    }
}