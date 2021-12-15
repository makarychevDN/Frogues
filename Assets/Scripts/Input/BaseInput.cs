using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInput : MonoBehaviour
{
    [SerializeField] protected Unit unit;
    public UnityEvent OnInputDone;

    public abstract void Act();
}
