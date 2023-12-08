using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class JumpOnCellAbility : AreaTargetAbility, IAbleToReturnIsPrevisualized
    {
        [SerializeField] protected int range = 3;
        [SerializeField] private float movementSpeed = 13;
        [SerializeField] private float movementHeight = 0.7f;

        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;
        [SerializeField] private AnimationCurve parabolaAnimationCurve;

        private Cell _hashedCell;
        private bool _isPrevisualizedNow;

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = range;
            if(_hashedCell != null)
                value ^= _hashedCell.GetHashCode();

            return value ^ GetHashCode();
        }

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, range, true, false).AbleToStepCellsOnly();
        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
            _isPrevisualizedNow = false;
        }

        public bool IsPrevisualizedNow() => _isPrevisualizedNow;

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            if (cells == null || cells.Count == 0 || cells[0] == null)
                return false;

            return IsResoursePointsEnough() && _usingArea.Contains(cells[0]);
        }

        public override void PrepareToUsing(List<Cell> cells)
        {
            CalculateUsingArea();
            _hashedCell = cells[0];
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            return PossibleToUseOnCells(cells) ? cells : null;
        }

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            _owner.Movable.Move(cells[0], movementSpeed, movementHeight);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.ActionPoints.PreSpendPoints(CalculateActionPointsCost);
            _owner.BloodPoints.PreSpendPoints(CalculateBloodPointsCost);

            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetAnimationCurveShape(_owner.SpriteParent.position, cells[0].transform.position,
                movementHeight * _owner.CurrentCell.DistanceToCell(cells[0]), parabolaAnimationCurve);
            cells[0].EnableSelectedByAbilityCellHighlight(new List<Cell> { cells[0] });
        }
    }
}