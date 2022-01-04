using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetAI : BaseInput
{
    [SerializeField] private UnitContainer targetContainer;
    [SerializeField] private Weapon activeWeapon;
    [SerializeField] private AbleToSkipTurn skipTurnModule;

    public override void Act()
    {
        if (activeWeapon.PossibleToHitExpectedTarget)
        {
            activeWeapon.Use();
            return;
        }

        OnInputDone.Invoke();
        skipTurnModule.AutoSkip();
    }
}
