using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Container<T> : MonoBehaviour
{
    [SerializeField] private T _content;
    public virtual T Content { get => _content; set => _content = value; }

    public bool isEmpty => Content == null;

}
