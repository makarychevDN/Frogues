using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnInput : BaseInput
{
    public UnityEvent OnInput;

    public override void Act()
    {
        OnInput.Invoke();
    }
}
