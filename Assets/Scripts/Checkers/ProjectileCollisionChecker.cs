using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileCollisionChecker : MonoBehaviour
{
    public Unit _unit;
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    public void Check()
    {
        if (_unit == null)
            _unit = GetComponentInParent<Unit>();

        if(!MapBasedOnTilemap._instance.GetUnitsLayerCellByCoordinates(_unit.currentCell._coordinates).isEmpty)
        {
            OnTrue.Invoke();
        }
        else
        {
            OnFalse.Invoke();
        }
    }
}
