using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public UnityEvent<int> OnTakePhisicsDamage;

    public void TakeDamage(int damageValue, DamageType damageType)
    {

    }
}

public enum DamageType
{
    Phisics = 0, Fire = 1, Cold = 2
}
