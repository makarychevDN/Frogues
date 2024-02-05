using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikedBallAbility : AreaTargetAbility, IAbleToReturnIsPrevisualized, IAbleToDealDamage
    {
        [SerializeField] DamageType damageType;
        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private LineRenderer lineFromOwnerToTargetCell;
        [SerializeField] private AudioSource audioSource;
        private Unit hashedTarget;
        private bool _isPrevisualizedNow;
        private List<Cell> _hashedSelectedArea;

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = _usingArea.Count;

            if (_hashedSelectedArea != null && _hashedSelectedArea[0] != null)
            {
                for (int i = 0; i < _hashedSelectedArea.Count; i++)
                {
                    value ^= _hashedSelectedArea[i].GetHashCode();
                }
            }

            return value ^ GetHashCode();
        }

        public override List<Cell> CalculateUsingArea() 
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, CellsTaker.ObstacleMode.onlyBigUnitsAreObstacles, false, true);
        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTargetCell.gameObject.SetActive(false);
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
            _hashedSelectedArea = cells;
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return null;

            return CellsTaker.TakeCellsLineWhichContainCell(_owner.CurrentCell, cells[0], CellsTaker.ObstacleMode.onlyBigUnitsAreObstacles, false, true);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            cells.ForEach(cell => cell.EnableSelectedByAbilityCellHighlight(cells));
            lineFromOwnerToTargetCell.gameObject.SetActive(true);
            lineFromOwnerToTargetCell.SetAnimationCurveShape(_owner.transform.position, cells.GetLast().transform.position, jumpHeight, EntryPoint.Instance.DefaultMovementCurve);

            var obstacle = GetObstacle(_owner, cells);
            if(obstacle != null && obstacle is not Barrier)
            {
                obstacle.Health.PreTakeDamage(CalculateDamage());
                obstacle.MaterialInstanceContainer.EnableOutline(true);
            }
        }

        private Unit GetObstacle(Unit user, List<Cell> line)
        {
            HexDir hexDir = CellsTaker.GetDirByStartAndEndCells(user.CurrentCell, line.Last());
            return line.Last().CellNeighbours.GetNeighborByHexDir(hexDir).Content;
        }

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(cells[0]);

            hashedTarget = GetObstacle(_owner, cells);
            _owner.Movable.OnMovementEnd.AddListener(DealDamageOnMovementStopped);
            _owner.Movable.Move(cells.Last(), speed, jumpHeight);
        }

        private void PlayImpactSound()
        {
            audioSource.Play();
        }

        private void DealDamageOnMovementStopped()
        {
            if (hashedTarget != null && hashedTarget is not Barrier)
            {
                hashedTarget.Health.TakeDamage(CalculateDamage(), false, _owner); 
            }

            hashedTarget = null;
            _owner.Movable.OnMovementEnd.RemoveListener(DealDamageOnMovementStopped);
            PlayImpactSound();
        }

        public int GetDefaultDamage() => 0;

        public DamageType GetDamageType() => damageType;

        public int CalculateDamage() => _owner.Stats.Spikes + _owner.Health.Block;
    }
}