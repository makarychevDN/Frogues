using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    [SerializeField] private IntContainer _hp, _lastTakenDamage;

    public void Apply()
    {
        _hp.Content -= _lastTakenDamage.Content;
    }
}