using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField] private IntContainer _lastTakenDamage, _hp, _armor;

    public UnityEvent OnTakePhisicsDamage;
    public UnityEvent OnTakeFireDamage;
    public UnityEvent OnTakeColdDamage;
    public UnityEvent OnHpEnded;

    public void TakeDamage(int damageValue, DamageType damageType)
    {
        TakeDamage(damageValue, damageType, false);
    }

    public void TakeDamage(int damageValue, DamageType damageType, bool ignoreArmor)
    {
        if (!ignoreArmor && _armor != null)
        {
            damageValue -= _armor.Content;
            Mathf.Clamp(damageValue, 0, 1000);
        }
        
        _lastTakenDamage.Content = damageValue;

        switch (damageType)
        {
            case DamageType.Phisics: OnTakePhisicsDamage.Invoke(); break;
            case DamageType.Fire: OnTakeFireDamage.Invoke(); break;
            case DamageType.Cold: OnTakeColdDamage.Invoke(); break;
        }

        if(_hp.Content <= 0)
        {
            OnHpEnded.Invoke();
        }
    }
}

public enum DamageType
{
    Phisics = 0, Fire = 1, Cold = 2
}
