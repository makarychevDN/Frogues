using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent OnCellInUnitsLayerBecameFull;
    public UnityEvent OnCellInUnitsLayerBecameEmpty;

    public void CellBecameFull()
    {
        OnCellInUnitsLayerBecameFull.Invoke();
    }

    public void CellBecameEmpty()
    {
        OnCellInUnitsLayerBecameEmpty.Invoke();
    }
}
