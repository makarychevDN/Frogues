using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : CostsActionPointsBehaviour
{
    [Space]
    [SerializeField] protected List<BaseCellsEffect> cellEffects;
    [SerializeField] protected PlayAnimation usingAnimation;
    [SerializeField] protected IntContainer damage;
    [SerializeField] protected DamageTypeContainer damageType;
    [Header("Target For AI Only")]
    public UnitContainer expectedTargetContainer;

    private void Awake()
    {
        usingAnimation?.OnAnimationPlayed.AddListener(ApplyCellEffects);
    }

    public abstract void Use();
    public abstract void ApplyCellEffects();
    public abstract void HighlightCells();

    public abstract bool PossibleToHitExpectedTarget { get; }
}
