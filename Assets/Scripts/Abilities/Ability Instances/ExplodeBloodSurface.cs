using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ExplodeBloodSurface : DefaultUnitTargetAbility
    {
        [SerializeField] private int radiusOfExplosion = 1;

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(target);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, target));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            TakeUnitsAroundSurfaceTarget(target).ForEach(targetInArea => ApplyEffectOnTargetInTheArea(targetInArea));
            target.AbleToDie.DieWithoutAnimation();
            OnEffectApplied.Invoke();
        } 

        private void ApplyEffectOnTargetInTheArea(Unit target)
        {
            target.Health.TakeDamage(CalculateDamage, ignoreArmor, _owner);

            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.effectIsConstantly));
            }
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (target != null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position - _owner.transform.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position - _owner.transform.position);

            TakeUnitsAroundSurfaceTarget(target).ForEach(targetInTheArea => targetInTheArea.Health.PreTakeDamage(CalculateDamage, ignoreArmor));
        }

        private List<Unit> TakeUnitsAroundSurfaceTarget(Unit surfaceTarget)
        {
            List<Cell> targetCells = new List<Cell> { surfaceTarget.CurrentCell };
            targetCells.AddRange(CellsTaker.TakeCellsAreaByRange(surfaceTarget.CurrentCell, radiusOfExplosion));
            return targetCells.ContentFromEachCellWioutNulls();
        }

        public override bool CheckItUsableOnDefaultUnit() => false;

        public override bool CheckItUsableOnBloodSurfaceUnit() => true;
    }
}