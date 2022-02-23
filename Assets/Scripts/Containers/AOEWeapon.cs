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
    private List<Cell> _hashedValidCells, _hashedSelectedCells;

    public override bool PossibleToHitExpectedTarget => 
        IsActionPointsEnough() && selectedCellTaker.Take()
            .Where(selectedCell => validCellTaker.Take()
            .Contains(selectedCell))
            .Any(selectedCell => selectedCell.Content == expectedTargetContainer.Content);

    public override bool PossibleToUse => IsActionPointsEnough() 
        && selectedCellTaker.Take()?
        .Where(selectedCell => validCellTaker.Take()
        .Contains(selectedCell)).ToList().Count != 0;
    
    
    public override void Use()
    {
        if (!actionPoints.CheckIsActionPointsEnough(defaultActionPointsCost.Content))
            return;

        if(usingAnimation == null)
            return;

        _hashedSelectedCells = selectedCellTaker.Take();
        _hashedValidCells = validCellTaker.Take();
        
        if(_hashedSelectedCells.Where(selectedCell => _hashedValidCells.Contains(selectedCell)).ToList().Count == 0)
            return;

        SpendActionPoints();
        usingAnimation.Play();
        OnUse.Invoke();
    }

    public override void HighlightCells()
    {
        if(validCellTaker.Take() == null)
            return;
        
        validCellTaker.Take().ForEach(cell => cell.EnableValidForAbilityCellHighlight(true));

        if(selectedCellTaker.Take() == null)
            return;

        var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
        cells.ForEach(cell => cell.EnableSelectedCellHighlight(true));

        cellEffects.Where(cellEffect => cellEffect as CellsEffectWithPreVisualization).ToList()
            .ForEach(cellEffect => (cellEffect as CellsEffectWithPreVisualization).PreVisualizeEffect(cells));
    }

    public override void ApplyCellEffects()
    {
        var cells = _hashedSelectedCells.Where(selectedCell => _hashedValidCells.Contains(selectedCell)).ToList();
        cellEffects.ForEach(effect => effect.ApplyEffect(cells));
    }
}
