using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeapon : Weapon
{
    public override bool PossibleToHitExpectedTarget => throw new System.NotImplementedException();

    public override void ApplyCellEffects()
    {
        throw new System.NotImplementedException();
    }

    public override void HighlightCells()
    {
        throw new System.NotImplementedException();
    }

    public override void Use()
    {
        if (!actionPoints.CheckIsActionPointsEnough(defaultActionPointsCost.Content))
            return;

        SpendActionPoints();
        usingAnimation.Play();
        OnUse.Invoke();
    }
}
