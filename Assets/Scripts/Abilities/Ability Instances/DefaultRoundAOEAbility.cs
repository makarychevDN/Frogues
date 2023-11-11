using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DefaultRoundAOEAbility : AreaTargetAbility, IAbleToReturnIsPrevisualized
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] private int damage;
        [SerializeField] private int usingRadius;
        [SerializeField] private int effectRadius;
        [SerializeField] private bool includeCellsOutOfUsingArea;
        private bool _isPrevisualizedNow;
        private List<Cell> _hashedSelectedArea;

        protected virtual int CalculateDamage => Extensions.CalculateDamageWithGameRules(damage, damageType, _owner.Stats);

        public override void PrepareToUsing(List<Cell> cells)
        {
            CalculateUsingArea();
            _hashedSelectedArea = cells;
        }

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, usingRadius);

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            if (cells == null || cells.Count == 0 || cells[0] == null)
                return false;

            return IsResoursePointsEnough() && _usingArea.Contains(cells[0]);
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
            cells.Where(cell => !cell.IsEmpty).ToList().ForEach(cell => cell.Content.Health.TakeDamage(CalculateDamage, _owner));
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound() => impactSoundSource.Play();

        public override void DisablePreVisualization()
        {
            _isPrevisualizedNow = false;
        }

        public override List<Cell> SelectCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return null;

            List<Cell> selectedCells = CellsTaker.TakeCellsAreaByRange(cells[0], effectRadius);
            selectedCells.Insert(0, cells[0]);

            if (!includeCellsOutOfUsingArea)
            {
                selectedCells = selectedCells.Where(cell => _usingArea.Contains(cell)).ToList();
            }

            return selectedCells;
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);

            foreach (var cell in cells) 
            {
                cell.EnableSelectedByAbilityCellHighlight(cells);

                if (!cell.IsEmpty)
                {
                    cell.Content.Health.PreTakeDamage(CalculateDamage);
                    cell.Content.MaterialInstanceContainer.EnableOutline(true);
                }
            }
        }

        public override void Init(Unit unit)
        {
            base.Init(unit);

            if (weaponIndex == WeaponIndexes.NoNeedToChangeWeapon)
                return;

            _owner.Animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, (int)weaponIndex);
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
        }

        public bool IsPrevisualizedNow() => _isPrevisualizedNow;

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
    }
}
