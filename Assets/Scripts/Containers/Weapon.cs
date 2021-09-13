using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CostsActionPointsBehaviour
{
    //coming soon
    [SerializeField] private List<BaseCellsEffect> cellEffects;

    public void Use()
    {
        cellEffects.ForEach(effect => effect.ApplyEffect());
    }
}
