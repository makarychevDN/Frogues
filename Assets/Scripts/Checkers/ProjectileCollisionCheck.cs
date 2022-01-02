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

        if(!Map.Instance.GetUnitsLayerCellByCoordinates(unit.Coordinates).IsEmpty)
        {
            OnTrue.Invoke();
            print("i killed myslef");
        }
        else
        {
            OnFalse.Invoke();
        }
    }
}
