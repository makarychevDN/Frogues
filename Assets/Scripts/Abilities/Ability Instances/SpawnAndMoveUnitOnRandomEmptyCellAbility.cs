using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveUnitOnRandomEmptyCellAbility : NonTargetAbility, IAbleToReturnRange
    {
        [SerializeField] private int radius;
        [SerializeField] private Unit unitPrefab;
        private List<Cell> _usingArea;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            var cell = _usingArea.GetRandomElement();
            StartCoroutine(ApplyEffect(timeBeforeImpact, cell));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        private List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        protected virtual IEnumerator ApplyEffect(float time, Cell cell)
        {
            yield return new WaitForSeconds(time);
            EntryPoint.Instance.SpawnUnit(unitPrefab, _owner, cell);
        }

        public override bool PossibleToUse()
        {
            return base.PossibleToUse() && CalculateUsingArea().EmptyCellsOnly().Count != 0;
        }

        private void PlayImpactSound() => impactSoundSource.Play();

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public int ReturnRange() => radius;
    }
}