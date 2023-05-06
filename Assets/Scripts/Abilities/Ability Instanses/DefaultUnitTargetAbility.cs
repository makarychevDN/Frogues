using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class DefaultUnitTargetAbility : UnitTargetAbility
    {
        [SerializeField] private int damage;
        [SerializeField] private int radius;

        [Header("Animation Setup")]
        [SerializeField] private WeaponIndexes weaponIndex = WeaponIndexes.NoNeedToChangeWeapon;
        [SerializeField] private bool needToRotateOwnersSprite = true;
        [SerializeField] private float timeBeforeImpact;
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private float delayBeforeImpactSound;
        [SerializeField] private AudioSource impactSoundSource;

        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override bool PossibleToUseOnUnit(Unit target)
        {
            if(target == null) 
                return false;

            CalculateUsingArea();
            return IsActionPointsEnough() && _usingArea.Contains(target.CurrentCell);
        }

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendActionPoints();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(target);
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.Attack);

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

        private void PlayImpactSound() => impactSoundSource.Play();

        public override void VisualizePreUseOnUnit(Unit target)
        {
            CalculateUsingArea();
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(damage);
            _owner.ActionPoints.PreSpendPoints(cost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position - _owner.transform.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position - _owner.transform.position);
            target.MaterialInstanceContainer.EnableOutline(true);
        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
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