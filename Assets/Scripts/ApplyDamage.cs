using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    [SerializeField] private IntContainer hp;
    [SerializeField] private IntContainer lastTakenDamage;

    public void Apply()
    {
        hp.Content -= lastTakenDamage.Content;
    }
}
