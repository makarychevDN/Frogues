using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnInput : BaseInput
{
    public UnityEvent OnInput;

    protected override void Act()
    {
        OnInput.Invoke();
    }
}
