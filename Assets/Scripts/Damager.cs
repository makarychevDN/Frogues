using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private int _damage;
    [SerializeField] private DamageType _damageType;
    public void DealDamage()
    {
        foreach (var cell in MapBasedOnTilemap._instance.GetCellsColumn(_unit.Coordinates))
        {
            if(!cell.isEmpty && cell.Content.GetComponentInChildren<Damagable>()!= null)
            {
                cell.Content.GetComponentInChildren<Damagable>().TakeDamage(_damage, _damageType);
            }
                
        }
    }
}
