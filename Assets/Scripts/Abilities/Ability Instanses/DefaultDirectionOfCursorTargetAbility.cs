using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DefaultDirectionOfCursorTargetAbility : DirectionOfCursorTargetAbility, IAbleToReturnIsPrevisualized
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] protected int damage;
        [SerializeField] protected int radius;
        [SerializeField] protected CollidersContainer collidersContainer;
        [SerializeField] private bool includeCellsOutOfUsingArea;
        private bool _isPrevisualizedNow;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override bool PossibleToUseInDirection(Vector3 cursorPosition)
        {
            return IsResoursePointsEnough();
        }

        public override List<Cell> SelectCells(Vector3 cursorPosition)
        {
            var cells = collidersContainer.Cells;

            if (!includeCellsOutOfUsingArea)
            {
                cells = cells.Where(cell => _usingArea.Contains(cell)).ToList();
            }

            return cells;
        }

        public override void UseInDirection(Vector3 cursorPosition)
        {
            if (!PossibleToUseInDirection(cursorPosition))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, SelectCells(cursorPosition)));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        protected virtual IEnumerator ApplyEffect(float time, List<Cell> cells)
        {
            yield return new WaitForSeconds(time);
            cells.Where(cell => !cell.IsEmpty).ToList().ForEach(cell => cell.Content.Health.TakeDamage(damage));
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound() => impactSoundSource.Play();

        public override void VisualizePreUseInDirection(Vector3 cursorPosition)
        {
            _isPrevisualizedNow = true;

            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            var cells = SelectCells(cursorPosition);
            cells.ForEach(cell => cell.EnableSelectedCellHighlight(true));
        }

        public bool IsPrevisualizedNow() => _isPrevisualizedNow;

        public override void DisablePreVisualization() => _isPrevisualizedNow = false;
    }
}