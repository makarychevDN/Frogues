﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrentlyActiveBehaviour : MonoBehaviour
{
    protected bool ActiveNow 
    {
        set 
        {
            if (value)
                CurrentlyActiveObjects.Add(this);
            else
                CurrentlyActiveObjects.Remove(this);
        }
    }
}
