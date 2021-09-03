using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInput : CurrentlyActiveBehaviour
{
    [SerializeField] protected Unit unit;
    public abstract void Act();
}
