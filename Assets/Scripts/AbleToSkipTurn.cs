using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbleToSkipTurn : MonoBehaviour
{
    [SerializeField] private Unit unit;
    public UnityEvent OnSkipTurn;

    public void AutoSkip()
    {
        if (!UnitsQueue.Instance.IsUnitCurrent(unit))
            return;

        if (unit.input as PlayerInput)
        {
            var input = unit.input as PlayerInput;
            input.DisableAllVisualizationFromPlayerOnMap();
            input.InputIsPossible = false;
        }

        OnSkipTurn.Invoke();
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
