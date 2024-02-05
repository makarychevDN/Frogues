using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    //todo remake it as nontarget ability
    public class SpawnAndMoveUnitOnRandomEmptyCellInRadiusAbility : AreaTargetAbility
    {
        [SerializeField] private int radius;
        [SerializeField] private Unit unitPrefab;

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            var selectedCell = _usingArea.GetRandomElement();            
            return selectedCell == null ? null : new List<Cell> { selectedCell };
        }

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            return IsResoursePointsEnough() && cells != null && cells.Count > 0;
        }

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(cells[0]);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, cells));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        protected virtual IEnumerator ApplyEffect(float time, List<Cell> cells)
        {
            yield return new WaitForSeconds(time);
            EntryPoint.Instance.SpawnUnit(unitPrefab, _owner, cells[0]);
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound() => impactSoundSource.Play();

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
        }

        public override List<Cell> CalculateUsingArea() =>
            _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius).EmptyCellsOnly();

        public override void DisablePreVisualization(){}

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            return 0;
        }

        public override void PrepareToUsing(List<Cell> cells)
        {
            CalculateUsingArea();
        }
    }
}