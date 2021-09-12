using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbleToSkipTurn : MonoBehaviour
{
    [SerializeField] private Unit unit;

    public void AutoSkip()
    {
        if(UnitsQueue.Instance.IsUnitCurrent(unit))
            UnitsQueue.Instance.ActivateNext();
    }

    public void SkipTurnWithAnimation()
    {
        if (UnitsQueue.Instance.IsUnitCurrent(unit))
            UnitsQueue.Instance.ActivateNext();
    }
}
