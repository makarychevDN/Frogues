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

            _owner.CurrentRoom.CurrentlyActiveObjects.Add(this);
            var cell = _usingArea.GetRandomElement();
            StartCoroutine(ApplyEffect(timeBeforeImpact, cell));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        private List<Cell> CalculateUsingArea() => _usingArea = _owner.CurrentRoom.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        protected virtual IEnumerator ApplyEffect(float time, Cell cell)
        {
            yield return new WaitForSeconds(time);
            var spawnedUnit = Extensions.SpawnUnit(unitPrefab, _owner, cell, _owner.CurrentRoom);
            spawnedUnit.IsSummoned = true;
        }

        public override bool PossibleToUse()
        {
            return base.PossibleToUse() && CalculateUsingArea().EmptyCellsOnly().Count != 0;
        }

        private void PlayImpactSound() => impactSoundSource.Play();

        private void RemoveCurremtlyActive() => _owner.CurrentRoom.CurrentlyActiveObjects.Remove(this);

        public int ReturnRange() => radius;
    }
}