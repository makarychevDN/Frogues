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
    [SerializeField] private IntContainer lastTakenDamage;
    [SerializeField] private PlayAnimation takeDamageAnimation;

    public UnityEvent OnTakeAnyDamage;
    public UnityEvent OnTakeUnblockedDamage;
    public UnityEvent OnTakePhisicsDamage;
    public UnityEvent OnTakeFireDamage;
    public UnityEvent OnTakeColdDamage;
    public UnityEvent OnHpEnded;

    private UnityEvent OnDamageApplyedToHpContainer = new UnityEvent();

    private void Awake()
    {
        if (takeDamageAnimation != null)
            OnDamageApplyedToHpContainer.AddListener(takeDamageAnimation.Play);
    }

    public void TakeDamage(int damageValue, DamageType damageType) => CalculateDamage(hp, damageValue, damageType, false);
    public void TakeDamage(int damageValue, DamageType damageType, bool ignoreArmor) => CalculateDamage(hp, damageValue, damageType, ignoreArmor);

    public void PretakeDamage(int damageValue, DamageType damageType) => CalculateDamage(preDamagedHp, damageValue, damageType, false);
    public void PretakeDamage(int damageValue, DamageType damageType, bool ignoreArmor) => CalculateDamage(preDamagedHp, damageValue, damageType, ignoreArmor);

    public void CalculateDamage(IntContainer containerToApplyDamage, int damageValue, DamageType damageType, bool ignoreArmor)
    {
        OnTakeAnyDamage.Invoke();

        if (!ignoreArmor && armor != null)
        {
            damageValue -= armor.Content;
            damageValue = Mathf.Clamp(damageValue, 0, 1000);
        }

        if (damageValue != 0)
            OnTakeUnblockedDamage.Invoke();
        
        lastTakenDamage.Content = damageValue;

        switch (damageType)
        {
            case DamageType.Phisics: OnTakePhisicsDamage.Invoke(); break;
            case DamageType.Fire: OnTakeFireDamage.Invoke(); break;
            case DamageType.Cold: OnTakeColdDamage.Invoke(); break;
        }

        if (lastTakenDamage.Content > 0 && containerToApplyDamage == hp)
            OnDamageApplyedToHpContainer.Invoke();

        containerToApplyDamage.Content -= lastTakenDamage.Content;
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
