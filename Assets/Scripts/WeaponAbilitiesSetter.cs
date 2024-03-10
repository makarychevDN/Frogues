using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using UnityEngine;

public class WeaponAbilitiesSetter : MonoBehaviour
{
    [SerializeField] private int prefabIndex;
    private List<BaseAbility> _abilities;

    public int PrefabIndex => prefabIndex;

    public void Init(Cell cell)
    {
        _abilities = GetComponentsInChildren<BaseAbility>().ToList();
        transform.position = cell.transform.position;
        cell.OnBecameFullByUnit.AddListener(SetWeaponAbilities);
    }

    public void SetWeaponAbilities(Unit unit)
    {
        unit.AbilitiesManager.RemoveAllWeaponAbilities();
        _abilities.ForEach(ability => ability.Init(unit));
        unit.AbilitiesManager.InvokeOnWeaponChanged();

        var weaponIndexContainer = unit.GetComponentInChildren<SelectedWeaponIndexContainer>();
        if(weaponIndexContainer != null )
        {
            weaponIndexContainer.Index = prefabIndex;
        }
    }
}
