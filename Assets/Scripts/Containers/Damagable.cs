using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField] private IntContainer hp;
    [SerializeField] private IntContainer preDamagedHp;
    [SerializeField] private IntContainer maxHP;
    [SerializeField] private IntContainer armor;
    public IntContainer Armor => armor;
    [SerializeField] private IntContainer lastTakenDamage;
    [SerializeField] private PlayAnimation takeDamageAnimation;

    public UnityEvent OnCalculateAnyDamage;
    public UnityEvent OnCalculateUnblockedDamage;
    public UnityEvent OnCalculatePhisicsDamage;
    public UnityEvent OnCalculateFireDamage;
    public UnityEvent OnCalculateColdDamage;

    public UnityEvent OnApplyAnyDamage;
    public UnityEvent OnApplyUnblockedDamage;
    public UnityEvent OnApplyPhisicsDamage;
    public UnityEvent OnApplyFireDamage;
    public UnityEvent OnApplyColdDamage;
    public UnityEvent OnHpEnded;

    private void Awake()
    {
        if (takeDamageAnimation != null)
            OnApplyUnblockedDamage.AddListener(takeDamageAnimation.Play);
    }

    public void TakeDamage(int damageValue, DamageType damageType) => CalculateDamage(hp, damageValue, damageType, false);
    public void TakeDamage(int damageValue, DamageType damageType, bool ignoreArmor) => CalculateDamage(hp, damageValue, damageType, ignoreArmor);

    public void PretakeDamage(int damageValue, DamageType damageType) => CalculateDamage(preDamagedHp, damageValue, damageType, false);
    public void PretakeDamage(int damageValue, DamageType damageType, bool ignoreArmor) => CalculateDamage(preDamagedHp, damageValue, damageType, ignoreArmor);

    private void CalculateDamage(IntContainer containerToApplyDamage, int damageValue, DamageType damageType, bool ignoreArmor)
    {
        //OnApplyAnyDamage.Invoke();

        if (!ignoreArmor && armor != null)
        {
            damageValue -= armor.Content;
            damageValue = Mathf.Clamp(damageValue, 0, 1000);
        }

        if (damageValue != 0)
            OnCalculateUnblockedDamage.Invoke();
        
        lastTakenDamage.Content = damageValue;

        switch (damageType)
        {
            case DamageType.Phisics: OnCalculatePhisicsDamage.Invoke(); break;
            case DamageType.Fire: OnCalculateFireDamage.Invoke(); break;
            case DamageType.Cold: OnCalculateColdDamage.Invoke(); break;
        }

        if (containerToApplyDamage == hp)
            ApplyEffects(damageType);

        containerToApplyDamage.Content -= lastTakenDamage.Content;
    }

    private void ApplyEffects(DamageType damageType)
    {
        OnApplyAnyDamage.Invoke();

        if (lastTakenDamage.Content > 0)
            OnApplyUnblockedDamage.Invoke();

        switch (damageType)
        {
            case DamageType.Phisics: OnApplyPhisicsDamage.Invoke(); break;
            case DamageType.Fire: OnApplyFireDamage.Invoke(); break;
            case DamageType.Cold: OnApplyColdDamage.Invoke(); break;
        }
    }

    public void ResetPreDamageValue()
    {
        preDamagedHp.Content = hp.Content;
    }

    public void CheckIsHpEnded()
    {
        if (hp.Content <= 0)
        {
            OnHpEnded.Invoke();
        }
    }
}

public enum DamageType
{
    Phisics = 0, Fire = 1, Cold = 2
}
