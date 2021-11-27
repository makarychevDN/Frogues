using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnKeyDownUnityEvent : MonoBehaviour
{
    [SerializeField] private KeyCode expectedKey;
    public UnityEvent OnKeyDown;

    void Update()
    {
        if (Input.GetKeyDown(expectedKey))
            OnKeyDown.Invoke();
    }
}
