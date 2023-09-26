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
        [SerializeField] private NearestHexDirectionToMouseFinder nearestHexDirectionToMouseFinder;
        private bool _isPrevisualizedNow;
        private List<Cell> _hashedSelectedCells;

        public override void PrepareToUsing(Vector3 cursorPosition)
        {
            CalculateUsingArea();
            _hashedSelectedCells = SelectCells(cursorPosition);
        }

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

            var nearestToCursorTranform = nearestHexDirectionToMouseFinder.FindNearestVectorToCursor(cursorPosition);
            collidersContainer.transform.position = nearestToCursorTranform.position;
            collidersContainer.transform.rotation = nearestToCursorTranform.rotation;

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

            foreach (var cell in cells)
            {
                cell.EnableSelectedByAbilityCellHighlight(cells);

                if (!cell.IsEmpty)
                {
                    cell.Content.Health.PreTakeDamage(damage);
                    cell.Content.MaterialInstanceContainer.EnableOutline(true);
                }
            }
        }

        public bool IsPrevisualizedNow() => _isPrevisualizedNow;

        public override void DisablePreVisualization() => _isPrevisualizedNow = false;

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = _usingArea.Count;

            if (_hashedSelectedCells != null && _hashedSelectedCells.Count > 0
                && _hashedSelectedCells[0] != null)
            {
                for (int i = 0; i < _hashedSelectedCells.Count; i++)
                {
                    value ^= _hashedSelectedCells[i].GetHashCode();
                }
            }

            return value ^ GetHashCode();
        }
    }
}