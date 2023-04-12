using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using UnityEngine;

public class WeaponAbilitiesSetter : MonoBehaviour
{
    [SerializeField] private List<BaseAbility> _abilities;

    private void Start()
    {
        _abilities = GetComponentsInChildren<BaseAbility>().ToList();
    }

    public void SetWeaponAbilities(Unit unit)
    {
        unit.AbilitiesManager.RemoveAllWeaponAbilities();
        _abilities.ForEach(ability => ability.Init(unit));
    }
}
