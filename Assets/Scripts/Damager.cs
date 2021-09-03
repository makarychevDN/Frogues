using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private int damage;
    [SerializeField] private DamageType damageType;
    public void DealDamage()
    {
        foreach (var cell in MapBasedOnTilemap.Instance.GetCellsColumn(unit.Coordinates))
        {
            if(!cell.IsEmpty && cell.Content.GetComponentInChildren<Damagable>()!= null)
            {
                cell.Content.GetComponentInChildren<Damagable>().TakeDamage(damage, damageType);
            }
                
        }
    }
}
