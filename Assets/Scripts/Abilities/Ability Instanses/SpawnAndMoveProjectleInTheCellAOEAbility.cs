using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectleInTheCellAOEAbility : AreaTargetAbility
    {
        [SerializeField] private int usingRadius;
        [SerializeField] private Unit projectilePrefab;
        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;
        [SerializeField] private AnimationCurve parabolaAnimationCurve;
        [SerializeField] private float parabolaHeight;

        private List<Cell> _selectedArea;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, usingRadius);

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
        }

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            if (cells == null || cells.Count == 0 || cells[0] == null)
                return false;

            return IsResoursePointsEnough() && _usingArea.Contains(cells[0]);
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return null;

            return cells;
        }

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            SpendResourcePoints();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(cells[0]);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, cells[0]));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound() => impactSoundSource.Play();

        protected IEnumerator ApplyEffect(float time, Cell target)
        {
            yield return new WaitForSeconds(time);
            EntryPoint.Instance.SpawnUnit(projectilePrefab, _owner, target);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            CalculateUsingArea();
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);

            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetAnimationCurveShape(_owner.transform.position, _owner.SpriteParent.position, cells[0].transform.position, 
                parabolaHeight * _owner.CurrentCell.DistanceToCell(cells[0]), parabolaAnimationCurve);
            cells[0].EnableSelectedCellHighlight(true);
        }
    }
}