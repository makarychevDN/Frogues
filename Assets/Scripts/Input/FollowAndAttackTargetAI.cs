using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetAI : BaseInput
    {
        [SerializeField] private UnitContainer targetContainer;
        [SerializeField] private Weapon activeWeapon;
        [SerializeField] private AbleToSkipTurn skipTurnModule;
        [SerializeField] private SpriteRotator spriteRotator;
        [SerializeField] private bool ignoreDefaultUnits, ignoreProjectiles, ignoreSurfaces;

        private List<Cell> _pathToTarget;

        private void Start()
        {
            unit.GetComponentInChildren<ActionPoints>().OnActionPointsEnded.AddListener(ClearPath);
        }

        public override void Act()
        {
            if (_pathToTarget == null)
                _pathToTarget = PathFinder.Instance.FindWay(unit.currentCell, targetContainer.Content.currentCell,
                    ignoreDefaultUnits, ignoreProjectiles, ignoreSurfaces);

            if (activeWeapon.PossibleToHitExpectedTarget)
            {
                spriteRotator.TurnByTarget(targetContainer.Content);
                activeWeapon.Use();
                return;
            }

            if (_pathToTarget != null && _pathToTarget.Count > 1)
            {
                unit.movable.Move(_pathToTarget[0]);
                _pathToTarget?.RemoveAt(0);
                return;
            }

            OnInputDone.Invoke();
            skipTurnModule.AutoSkip();
            ClearPath();
        }

        public void ClearPath()
        {
            _pathToTarget = null;
        }
    }
}