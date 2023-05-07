using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class KickAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private LineRenderer lineFromTargetUnitToTargetCell;
        [SerializeField] private AnimationCurve parabolaCurve;
        [SerializeField] private float parabolaHeight;

        public override bool PossibleToUseOnUnit(Unit target)
        {            
            var preResult = base.PossibleToUseOnUnit(target);

            if (!preResult)
                return false;

            var hexDir = _owner.CurrentCell.CellNeighbours.GetHexDirByNeighbor(target.CurrentCell);
            var targetCell = target.CurrentCell.CellNeighbours.GetNeighborByHexDir(hexDir);
            return target.Movable.IsPossibleToMoveOnCell(targetCell);
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            base.VisualizePreUseOnUnit(target);

            if (!PossibleToUseOnUnit(target))
                return;

            var hexDir = _owner.CurrentCell.CellNeighbours.GetHexDirByNeighbor(target.CurrentCell);
            var targetCell = target.CurrentCell.CellNeighbours.GetNeighborByHexDir(hexDir);

            lineFromTargetUnitToTargetCell.gameObject.SetActive(true);
            var stepOnCurve = 1f / lineFromTargetUnitToTargetCell.positionCount;
            for (int i = 0; i < lineFromTargetUnitToTargetCell.positionCount; i++)
            {
                var pos = PositionOnCurveCalculator.Calculate(target.CurrentCell, targetCell, parabolaCurve, stepOnCurve * i, parabolaHeight);
                lineFromTargetUnitToTargetCell.SetPosition(i, pos - _owner.transform.position);
            }
        }

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendActionPoints();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(target);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            var hexDir = _owner.CurrentCell.CellNeighbours.GetHexDirByNeighbor(target.CurrentCell);
            var targetCell = target.CurrentCell.CellNeighbours.GetNeighborByHexDir(hexDir);

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, target, targetCell));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        private void PlayImpactSound()
        {
            if (impactSoundSource != null)
                impactSoundSource.Play();
        }

        protected IEnumerator ApplyEffect(float time, Unit target, Cell targetCell)
        {
            yield return new WaitForSeconds(time);
            target.Movable.Move(targetCell, 10, 0.4f, true, false);
        }

        public override void DisablePreVisualization()
        {
            base.DisablePreVisualization();
            lineFromTargetUnitToTargetCell.gameObject.SetActive(false);
        }
    }
}