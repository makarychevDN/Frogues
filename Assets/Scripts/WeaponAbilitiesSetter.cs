using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using UnityEngine;

public class WeaponAbilitiesSetter : MonoBehaviour
{
    private List<IAbility> _abilities;

    private void Start()
    {
        _abilities = GetComponentsInChildren<IAbility>().ToList();
    }

    public void SetWeaponAbilities(Unit unit)
    {
        unit.AbilitiesManager.RemoveAllWeaponAbilities();
        _abilities.ForEach(ability => ability.Init(unit));
    }
}
