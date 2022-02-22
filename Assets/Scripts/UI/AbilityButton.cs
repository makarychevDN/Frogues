using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private AOEWeapon ability;
    [SerializeField] private bool usingNow;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Image>().material;
    }

    public bool UsingNow
    {
        get => usingNow;
        set
        {
            usingNow = value;
            _material.SetInt("_AbilityUsingNow", value ? 1 : 0);
        }
    }
    
    

}
