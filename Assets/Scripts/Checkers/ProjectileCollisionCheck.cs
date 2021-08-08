using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileCollisionCheck : MonoBehaviour
{
    public Unit _unit;
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    public void Check()
    {
        if (_unit == null)
            _unit = GetComponentInParent<Unit>();

        if(!MapBasedOnTilemap.Instance.GetUnitsLayerCellByCoordinates(_unit.Coordinates).isEmpty)
        {
            OnTrue.Invoke();
        }
        else
        {
            OnFalse.Invoke();
        }
    }
}
