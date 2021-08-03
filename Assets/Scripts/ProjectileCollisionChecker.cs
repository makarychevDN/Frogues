using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileCollisionChecker : MonoBehaviour
{
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    public void Check()
    {
        Cell temp = FindObjectOfType<MapBasedOnTilemap>().GetCellInDefaultUnitsLayerByUnit(GetComponentInParent<Unit>());
        var temptemp = temp.Content;

        if(temptemp == null)
        {
            OnTrue.Invoke();
        }
        else
        {
            OnFalse.Invoke();
        }
    }
}
