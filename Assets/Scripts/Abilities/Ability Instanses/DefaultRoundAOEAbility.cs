using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FroguesFramework
{
    public class DefaultRoundAOEAbility : AreaTargetAbility
    {
        [SerializeField] private int damage;
        [SerializeField] private int usingRadius;
        [SerializeField] private int effectRadius;
        [SerializeField] private bool includeCellsOutOfUsingArea;

        [Header("Animation Setup")]
        [SerializeField] private AbilityAnimatorTriggers abilityAnimatorTrigger;
        [SerializeField] private WeaponIndexes weaponIndex = WeaponIndexes.NoNeedToChangeWeapon;
        [SerializeField] private bool needToRotateOwnersSprite = true;
        [SerializeField] private float timeBeforeImpact;
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private float delayBeforeImpactSound;
        [SerializeField] private AudioSource impactSoundSource;

        private List<Cell> _selectedArea;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, usingRadius);

        public override bool PossibleToUseOnCells(List<Cell> cells)
        {
            if (cells == null || cells.Count == 0 || cells[0] == null)
                return false;

            return IsActionPointsEnough() && _usingArea.Contains(cells[0]);
        }

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            SpendActionPoints();

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
            cells.Where(cell => !cell.IsEmpty).ToList().ForEach(cell => cell.Content.Health.TakeDamage(damage));
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound() => impactSoundSource.Play();

        public override void DisablePreVisualization() { }

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
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.ActionPoints.PreSpendPoints(cost);

            foreach (var cell in cells) 
            {
                cell.EnableSelectedCellHighlight(true);

                if (!cell.IsEmpty)
                {
                    cell.Content.Health.PreTakeDamage(damage);
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
    }
}
