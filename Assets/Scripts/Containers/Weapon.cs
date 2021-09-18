using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : CostsActionPointsBehaviour
{
    [Space]
    [SerializeField] protected List<BaseCellsEffect> cellEffects;

    public abstract void Use();
    public abstract void HighlightCells();
}
