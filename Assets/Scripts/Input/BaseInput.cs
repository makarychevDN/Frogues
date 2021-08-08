using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInput : CurrentlyActiveBehaviour
{
    [SerializeField] protected Unit _unit;
    public bool _inputIsPossible;

    protected virtual void Update()
    {
        if(_inputIsPossible && !CurrentlyActiveObjects.SomethingIsActNow)
        {
            Act();
        }
    }

    protected virtual void Act()
    {

    }
}
