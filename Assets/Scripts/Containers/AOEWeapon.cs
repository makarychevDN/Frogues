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
    //[SerializeField] private BoolContainer ignoreArmor;

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
        validCellTaker.Take().ForEach(cell => cell.EnableValidateCellHighlight(true));

        var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
        cells.ForEach(cell => cell.EnableSelectedCellHighlight(true));

        cellEffects.Where(cellEffect => cellEffect as CellsEffectWithPreVisualization).ToList()
            .ForEach(cellEffect => (cellEffect as CellsEffectWithPreVisualization).PreVisualizeEffect(cells));
    }

    public override void ApplyCellEffects()
    {
        var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
        cellEffects.ForEach(effect => effect.ApplyEffect(cells));
    }
}
