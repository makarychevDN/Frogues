using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AOEWeapon : Weapon
{
    [Space]
    [SerializeField] private BaseCellsTaker validCellTaker;
    [SerializeField] private BaseCellsTaker selectedCellTaker;

    public UnityEvent OnUse;

    public override void Use()
    {
        if (!actionPoints.CheckIsActionPointsEnough(defaultActionPointsCost.Content))
            return;

        SpendActionPoints();
        usingAnimation.Play();
        OnUse.Invoke();
    }

    public override void HighlightCells()
    {
        Map.Instance.allCells.ForEach(cell => cell.DisableAllVisualization());
        validCellTaker.Take().ForEach(cell => cell.EnableValidateCellHighlight(true));
        selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList().ForEach(cell => cell.EnableSelectedCellHighlight(true));
    }

    public override void ApplyCellEffects()
    {
        var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
        cellEffects.ForEach(effect => effect.ApplyEffect(cells));
    }
}
