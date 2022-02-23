using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllAbilityButtonsMaterialEnabler : MonoBehaviour
{
    public void Enable(bool value)
    {
        //FindObjectsOfType<AbilityButton>().ToList().ForEach(button => button.UsingNow = value);
    }

    private void Start()
    {
        Enable(false);
    }
}
