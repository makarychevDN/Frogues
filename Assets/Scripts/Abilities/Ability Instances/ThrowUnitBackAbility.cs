using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ThrowUnitBackAbility : AreaTargetAbility, IAbleToReturnIsPrevisualized, IAbleToHashUnitTarget
    {
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;

        [SerializeField] private float movementSpeed = 13;
        [SerializeField] private float movementHeight = 0.7f;

        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;
        [SerializeField] private AnimationCurve parabolaAnimationCurve;

        private Cell _hashedCell;
        private bool _isPrevisualizedNow;
        private Unit _hashedTarget;

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = 1;
            if (_hashedCell != null)
                value ^= _hashedCell.GetHashCode();

            return value ^ GetHashCode();
        }

        public override List<Cell> CalculateUsingArea()
        {
            HexDir direction = _hashedTarget.CurrentCell.CellNeighbours.GetHexDirByNeighbor(_owner.CurrentCell);
            return _usingArea = CellsTaker.TakeCellsLineInDirection(_owner.CurrentCell, direction, CellsTaker.ObstacleMode.onlyBigUnitsAreObstacles, false, false);
        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
            _isPrevisualizedNow = false;
        }

        public void HashUnitTargetAndCosts(Unit target) => _hashedTarget = target;

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

            _hashedTarget.Movable.OnMovementEnd.AddListener(DealDamage);
            _hashedTarget.Movable.Move(cells[0], movementSpeed, movementHeight);
            _owner.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);

            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.transform.position = _hashedTarget.transform.position;
            lineFromOwnerToTarget.SetAnimationCurveShape(_hashedTarget.SpriteParent.position, cells[0].transform.position,
                movementHeight * _hashedTarget.CurrentCell.DistanceToCell(cells[0]), parabolaAnimationCurve);
            cells[0].EnableSelectedByAbilityCellHighlight(new List<Cell> { cells[0] });

            _hashedTarget.Health.PreTakeDamage(CalculateDamage);
        }

        public override bool IsIgnoringDrawingFunctionality() => true;

        private void DealDamage()
        {
            _hashedTarget.Health.TakeDamage(CalculateDamage, _owner);
            _hashedTarget.Movable.OnMovementEnd.RemoveListener(DealDamage);
        }

        public void HashUnitTargetAndCosts(Unit target, int actionPointsCost, int bloodPointsCost)
        {
            throw new System.NotImplementedException();
        }

        private int CalculateDamage => Extensions.CalculateOutgoingDamageWithGameRules(damage, damageType, _owner.Stats);
    }
}