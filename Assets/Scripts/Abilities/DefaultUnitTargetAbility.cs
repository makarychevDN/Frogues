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
        [SerializeField] private float timeBeforeImpact;
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private AudioSource impactSoundSource;

        [Header("Previsualization Setup")]
        [SerializeField] private LineRenderer lineFromOwnerToTarget;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override bool PossibleToUseOnUnit(Unit target)
        {
            CalculateUsingArea();
            return IsActionPointsEnough() && _usingArea.Contains(target.CurrentCell);
        }

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendActionPoints();
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.Attack);
            impactSoundSource.Play();

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, target));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            target.Health.TakeDamage(damage);
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public override void VisualizePreUseOnUnit(Unit target)
        {
            CalculateUsingArea();
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (target == null)
                return;

            target.Health.PreTakeDamage(damage);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position - _owner.transform.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position - _owner.transform.position);

            //lineFromOwnerToTarget.gameObject.SetActive(true);


        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
        }
    }
}