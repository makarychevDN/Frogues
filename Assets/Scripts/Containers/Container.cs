using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Container<T> : MonoBehaviour
{
    [SerializeField] private T content;

    public virtual T Content
    {
        get => content;
        set => content = value;
    }

    public virtual bool IsEmpty => Content == null;
}
