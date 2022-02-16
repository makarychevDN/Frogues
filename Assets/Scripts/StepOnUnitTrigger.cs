using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepOnUnitTrigger : MonoBehaviour
{
    public UnityEvent OnStep;
    public UnityEvent<Unit> OnStepByUnit;
    public UnityEvent<List<Cell>> OnStepOnCell;

    public void Run(Unit steppedUnit)
    {
        OnStep.Invoke();
        OnStepByUnit.Invoke(steppedUnit);
        OnStepOnCell.Invoke(new List<Cell>() {steppedUnit.currentCell});
    }
}
