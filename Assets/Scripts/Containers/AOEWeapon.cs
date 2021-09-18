using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AOEWeapon : Weapon
{
    [Space]
    [SerializeField] private BaseCellsTaker validCellTaker;
    [SerializeField] private BaseCellsTaker selectedCellTaker;

    public override void Use()
    {
        if (!actionPoints.CheckIsActionPointsEnough(defaultActionPointsCost.Content))
            return;

        var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
        cellEffects.ForEach(effect => effect.ApplyEffect(cells));
    }

    public override void HighlightCells()
    {
        Map.Instance.allCells.ForEach(cell => cell.DisableAllVisualization());
        validCellTaker.Take().ForEach(cell => cell.EnableValidateCellHighlight(true));
        selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList().ForEach(cell => cell.EnableSelectedCellHighlight(true));
    }
}
