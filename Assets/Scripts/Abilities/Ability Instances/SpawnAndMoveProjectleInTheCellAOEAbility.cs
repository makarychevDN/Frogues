using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectleInTheCellAOEAbility : AreaTargetAbility, IAbleToReturnIsPrevisualized, IAbleToReturnRange,
        IAbleToApplyStrenghtModificator, IAbleToApplyIntelligenceModificator, IAbleToApplyDexterityModificator, IAbleToApplyDefenceModificator
    {
        [SerializeField] private int usingRadius;
        [SerializeField] private Unit projectilePrefab;
        [SerializeField] private List<StatEffect> addtionalDebufs;

        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;
        [SerializeField] private AnimationCurve parabolaAnimationCurve;
        [SerializeField] private float parabolaHeight;

        private Cell _hashedTargetCell;
        private bool _isPrevisualizedNow;

        public override void PrepareToUsing(List<Cell> cells)
        {
            CalculateUsingArea();
            _hashedTargetCell = cells[0];
        }

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, usingRadius);

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
            _isPrevisualizedNow = false;
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
            SetCooldownAsAfterUse();

            if (needToRotateOwnersSprite) 
                _owner.SpriteRotator.TurnAroundByTarget(cells[0]);

            if(healthCost == 0)
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
            var spawnedUnit = EntryPoint.Instance.SpawnUnit(projectilePrefab, _owner, target);
            spawnedUnit.Movable.OnMovementEndOnCell.AddListener(ApplyWeaknessEffectToUnitOnCell);
        }

        private void ApplyWeaknessEffectToUnitOnCell(Cell cell)
        {
            addtionalDebufs.ForEach(statEffect => cell.Content.Stats.AddStatEffect(new StatEffect(statEffect)));
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
            lineFromOwnerToTarget.SetAnimationCurveShape(_owner.SpriteParent.position, cells[0].transform.position, 
                parabolaHeight * _owner.CurrentCell.DistanceToCell(cells[0]), parabolaAnimationCurve);
            cells[0].EnableSelectedByAbilityCellHighlight(new List<Cell> { cells[0] });
        }

        public bool IsPrevisualizedNow() => _isPrevisualizedNow;

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = _usingArea.Count;

            if (_hashedTargetCell != null)
                value ^= _hashedTargetCell.GetHashCode();

            return value ^ GetHashCode();
        }

        public int ReturnRange() => usingRadius;

        #region IAbleToApplyDefenceModificator
        public int GetDefenceModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.defence);

        public int GetdeltaOfDefenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.defence);

        public int GetTimeToEndOfDefenceEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.defence);

        public bool GetDefenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.defence);
        #endregion

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.strength);

        public int GetDeltaOfStrenghtValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.strength);

        public int GetTimeToEndOfStrenghtEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.strength);

        public bool GetStrenghtEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.strength);
        #endregion

        #region IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.intelligence);

        public int GetDeltaOfIntelligenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.intelligence);

        public int GetTimeToEndOfIntelligenceEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.intelligence);

        public bool GetIntelligenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.intelligence);
        #endregion

        #region IAbleToApplyDexterityModificator
        public int GetDexterityModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.dexterity);

        public int GetDeltaOfDexterityValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.dexterity);

        public int GetTimeToEndOfDexterityEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.dexterity);

        public bool GetDexterityEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.dexterity);
        #endregion
    }
}