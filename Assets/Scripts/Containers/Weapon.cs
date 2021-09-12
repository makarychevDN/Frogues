using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CostsActionPointsBehaviour
{
    [SerializeField] private BaseCellsTaker baseCellsTaker;
    [SerializeField] private List<BaseCellsEffect> cellEffects;

    public void Use()
    {
        cellEffects.ForEach(effect => effect.ApplyEffect());
    }
}
