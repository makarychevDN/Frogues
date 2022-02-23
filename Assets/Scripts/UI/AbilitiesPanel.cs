using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesPanel : MonoBehaviour
{
    public static AbilitiesPanel Instance;
    public List<PlayerAbilityButtonSlot> abilitySlots;

    private void Start()
    {
        Instance = this;
    }
}
