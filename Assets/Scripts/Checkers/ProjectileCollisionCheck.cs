using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileCollisionCheck : MonoBehaviour
{
    public Unit unit;
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    public void Check()
    {
        if (unit == null)
            unit = GetComponentInParent<Unit>();

        if(!MapBasedOnTilemap.Instance.GetUnitsLayerCellByCoordinates(unit.Coordinates).IsEmpty)
        {
            OnTrue.Invoke();
        }
        else
        {
            OnFalse.Invoke();
        }
    }
}
