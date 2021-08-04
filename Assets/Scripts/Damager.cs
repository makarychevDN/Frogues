using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public Unit _unit;
    public void DealDamage()
    {
        foreach (var cell in MapBasedOnTilemap._instance.GetCellsColumn(_unit.currentCell._coordinates))
        {
            if(!cell.isEmpty)
                Destroy(cell.Content.gameObject);
        }
    }
}
