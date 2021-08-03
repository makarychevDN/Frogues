using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public void DealDamage()
    {
        var temp = FindObjectOfType<MapBasedOnTilemap>().GetCellInDefaultUnitsLayerByUnit(GetComponentInParent<Unit>());
        if (temp.Content != null)
        {
            temp.Content.gameObject.SetActive(false);
        }
    }
}
