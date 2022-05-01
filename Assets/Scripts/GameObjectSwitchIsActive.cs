using System;
using System.Collections;
using System.Collections.Generic;
using FroguesFramework;
using UnityEngine;

public class GameObjectSwitchIsActive : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void Awake()
    {
        if (target == null)
            target = gameObject;
    }

    public void Switch()
    {
        target.SwitchActive();
    }
}
