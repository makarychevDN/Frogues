using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTriggerOnUnitsLayerCellFilled : MonoBehaviour
{
    [SerializeField] private Cell _cell;

    public void TriggerOnBecameFull()
    {
        if (_cell.isEmpty)
            return;

        var temp = _cell.Content.GetComponentInChildren<Trigger>();
        if (temp != null)
            temp.CellBecameFull();
    }

    public void TriggerOnBecameEmpty()
    {
        if (_cell.isEmpty)
            return;

        var temp = _cell.Content.GetComponentInChildren<Trigger>();
        if (temp != null)
            temp.CellBecameEmpty();
    }
}
