using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikedBallAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        private Unit hashedTarget;

        public override List<Cell> CalculateUsingArea()
        {
            var cells = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, CellsTaker.ObstacleMode.onlyBigUnitsAreObstacles, true, true);
            var closestToUserCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, 1, true, false);
            foreach ( var cell in closestToUserCells)
            {
                cells.Remove(cell);
            }
            return _usingArea = cells;
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            base.VisualizePreUseOnUnit(target);

            if (!PossibleToUseOnUnit(target))
                return;

            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position - _owner.transform.position);
            Cell targetCell = CellsTaker.GetCellBeforeOtherCellInDirection(_owner.CurrentCell, target.CurrentCell);
            lineFromOwnerToTarget.SetPosition(1, targetCell.transform.position - _owner.transform.position);
        }

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(target);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact, target));
        }

        protected override int CalculateDamage => Extensions.CalculateDamageWithGameRules(_owner.Health.Block, damageType, _owner.Stats);

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);
            hashedTarget = target;
            _owner.Movable.OnMovementEnd.AddListener(DealDamageOnMovementStopped);
            Cell targetCell = CellsTaker.GetCellBeforeOtherCellInDirection(_owner.CurrentCell, target.CurrentCell);
            _owner.Movable.Move(targetCell, speed, jumpHeight);
            EntryPoint.Instance.DisableAllPrevisualization();
        }

        private void DealDamageOnMovementStopped()
        {
            hashedTarget.Health.TakeDamage(CalculateDamage, ignoreArmor, _owner);
            OnEffectApplied.Invoke();
            hashedTarget = null;
            _owner.Movable.OnMovementEnd.RemoveListener(DealDamageOnMovementStopped);
            PlayImpactSound();
        }
    }
}