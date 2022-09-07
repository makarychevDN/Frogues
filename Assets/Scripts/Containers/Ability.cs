using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class Ability : Weapon
    {
        [Space] [SerializeField] private BaseCellsTaker validCellTaker;
        [SerializeField] private BaseCellsTaker selectedCellTaker;
        [Header("Addition Cell Taker Setup")]
        [SerializeField] private bool additionalCellsIgnoreValidBorders;
        [SerializeField] private BaseCellsTaker additionalToSelectedCellTaker;
        [SerializeField] private AbilityButton abilityButtonPrefab;
        private List<Cell> /*_hashedValidCells,*/ _hashedSelectedCells;
        
        public override bool PossibleToHitExpectedTarget =>
            IsActionPointsEnough() && selectedCellTaker.Take()
                .Where(selectedCell => validCellTaker.Take()
                    .Contains(selectedCell))
                .Any(selectedCell => selectedCell.Content == expectedTargetContainer.Content);

        public override bool PossibleToUse => IsActionPointsEnough()
                                              && selectedCellTaker.Take()?
                                                  .Where(selectedCell => validCellTaker.Take()
                                                      .Contains(selectedCell)).ToList().Count != 0;

        public AbilityButton AbilityButtonPrefab => abilityButtonPrefab;

        public override void Use()
        {
            if (!actionPoints.CheckIsActionPointsEnough(defaultActionPointsCost.Content))
                return;

            if (usingAnimation == null)
                return;

            _hashedSelectedCells = CalculateAbilityCells();

            if (_hashedSelectedCells.Count == 0)
                return;

            SpendActionPoints();
            usingAnimation.Play();
            OnUse.Invoke();
        }

        public override void HighlightCells()
        {
            var validCells = validCellTaker.Take();
            
            if (validCells == null)
                return;

            validCells.ForEach(cell => cell.EnableValidForAbilityCellHighlight(validCells));

            if (selectedCellTaker.Take() == null)
                return;

            var cells = CalculateAbilityCells();
            cells.ForEach(cell => cell.EnableSelectedCellHighlight(true));

            cellEffects.Where(cellEffect => cellEffect as CellsEffectWithPreVisualization).ToList()
                .ForEach(cellEffect => (cellEffect as CellsEffectWithPreVisualization).PreVisualizeEffect(cells));
        }

        public override void ApplyCellEffects()
        {
            cellEffects.ForEach(effect => effect.ApplyEffect(_hashedSelectedCells));
        }

        private List<Cell> CalculateAbilityCells()
        {
            var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell))
                .ToList();

            if (cells.Count == 0 || additionalToSelectedCellTaker == null)
                return cells;

            List<Cell> additionalCells;
            if (!additionalCellsIgnoreValidBorders)
            {
                additionalCells = additionalToSelectedCellTaker.Take()
                    .Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
            }
            else
            {
                additionalCells = additionalToSelectedCellTaker.Take();
            }
            
            cells.AddUnique(additionalCells);

            return cells;
        }
    }
}
