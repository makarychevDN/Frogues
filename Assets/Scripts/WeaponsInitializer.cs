using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WeaponsInitializer : MonoBehaviour
    {
        [SerializeField] private List<WeaponAbilitiesSetter> weaponSetterPrefabs;
        [SerializeField] private List<Vector2Int> coordinatesOfCells;

        void Start()
        {
            EntryPoint.Instance.MetaPlayer.GetComponentInChildren<SelectedWeaponIndexContainer>();
            int count = 0;

            var weaponSetterWithCurrentWeapon = weaponSetterPrefabs
                .FirstOrDefault(weaponSetter => weaponSetter.PrefabIndex == EntryPoint.Instance.MetaPlayer.GetComponentInChildren<SelectedWeaponIndexContainer>().Index);

            if(weaponSetterWithCurrentWeapon != null)
            {
                SpawnAndInitWeaponSetterPrefab(weaponSetterWithCurrentWeapon, EntryPoint.Instance.Map.GetCell(coordinatesOfCells[count]));
                weaponSetterPrefabs.Remove(weaponSetterWithCurrentWeapon);
                count++;
            }

            while(count < coordinatesOfCells.Count)
            {
                var prefab = weaponSetterPrefabs.GetRandomElement();
                SpawnAndInitWeaponSetterPrefab(prefab, EntryPoint.Instance.Map.GetCell(coordinatesOfCells[count]));
                weaponSetterPrefabs.Remove(prefab);
                count++;
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