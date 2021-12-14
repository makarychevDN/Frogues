using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInput : MonoBehaviour
{
    [SerializeField] protected Unit unit;
    public abstract void Act();
}
