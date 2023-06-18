using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class DefaultUnitTargetAbility : UnitTargetAbility, IAbleToBeNativeAttack, IAbleToReturnIsPrevisualized
    {
        [SerializeField] protected int damage;
        [SerializeField] protected int radius;
        [SerializeField] protected bool isNativeAttack;

        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;

        private bool _isPrevisualizedNow;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override bool PossibleToUseOnUnit(Unit target)
        {
            if(target == null) 
                return false;

            CalculateUsingArea();
            return IsResoursePointsEnough() && _usingArea.Contains(target.CurrentCell);
        }

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendResourcePoints();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(target);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, target));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        protected virtual IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            target.Health.TakeDamage(damage);
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound()
        {
            if(impactSoundSource != null)
                impactSoundSource.Play();
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            CalculateUsingArea();
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if(target!= null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(damage);
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position - _owner.transform.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position - _owner.transform.position);
        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
            _isPrevisualizedNow = false;
        }

        public override void Init(Unit unit)
        {
            base.Init(unit);

            if (weaponIndex == WeaponIndexes.NoNeedToChangeWeapon)
                return;

            _owner.Animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, (int)weaponIndex);
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
        }

        public bool IsNativeAttack()
        {
            return isNativeAttack;
        }

        public void SetAsCurrentNativeAttack()
        {
            var ableToHaveNativeAttack = _owner.ActionsInput as IAbleToHaveNativeAttack;
            if (ableToHaveNativeAttack != null)
                ableToHaveNativeAttack.SetCurrentNativeAttack(this);
        }

        public bool IsPrevisualizedNow()
        {
            return _isPrevisualizedNow;
        }
    }
}