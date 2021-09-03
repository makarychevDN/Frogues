using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Container<T> : MonoBehaviour
{
    [FormerlySerializedAs("_content")] [SerializeField] private T content;

    public virtual T Content
    {
        get => content;
        set => content = value;
    }

    public bool IsEmpty => Content == null;
}
