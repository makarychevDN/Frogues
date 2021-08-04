using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Container<T> : MonoBehaviour
{
    [SerializeField] private T _content;

    public virtual T Content
    {
        get => _content;
        set
        {
            _content = value;
            if (_content == null)
            {
                OnBecameEmpty.Invoke();
            }
            else
            {
                OnBecameFull.Invoke();
            }   
        }
    }

    public bool isEmpty => Content == null;

    public UnityEvent OnBecameFull;
    public UnityEvent OnBecameEmpty;
}
