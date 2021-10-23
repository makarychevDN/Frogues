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
        var input = unit.input;
        if (input != null && input as PlayerInput)
            (input as PlayerInput).InputIsPossible = false;

        if (UnitsQueue.Instance.IsUnitCurrent(unit))
            UnitsQueue.Instance.ActivateNext();
    }
}
