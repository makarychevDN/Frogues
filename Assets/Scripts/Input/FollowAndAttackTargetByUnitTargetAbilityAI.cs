using UnityEngine;

namespace FroguesFramework
{
    public class FollowAndAttackTargetByUnitTargetAbilityAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility unitTargetAbilty;
        private Unit _unit;
        private MovementAbility _movementAbility;
        private ActionPoints _actionPoints;
        private AbleToSkipTurn _ableToSkipTurn;

        public void Act()
        {
            if (unitTargetAbilty.PossibleToUseOnUnit(target))
            {
                unitTargetAbilty.UseOnUnit(target);
                return;
            }

            var path = EntryPoint.Instance.PathFinder.FindWayExcludeLastCell(_unit.CurrentCell, target.CurrentCell, false, false, false);

            if (path == null || path.Count == 0 || !_actionPoints.IsActionPointsEnough(_movementAbility.GetCost()))
            {
                _ableToSkipTurn.AutoSkip();
                return;
            }

            _movementAbility.TargetCell = path[0];
            _movementAbility.Use();
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _movementAbility = _unit.MovementAbility;
            _actionPoints = _unit.ActionPoints;
            _ableToSkipTurn = _unit.AbleToSkipTurn;
            unitTargetAbilty.Init(_unit);

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}