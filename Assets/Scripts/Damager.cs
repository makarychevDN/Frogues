using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    public void DealDamage()
    {
        foreach (var cell in MapBasedOnTilemap._instance.GetCellsColumn(_unit.Coordinates))
        {
            if(!cell.isEmpty)
                Destroy(cell.Content.gameObject);
        }
    }
}
