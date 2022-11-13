using UnityEngine;

namespace FroguesFramework
{
    public class FollowTargetAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        private Unit _unit;
        private MovementAbility _movementAbility;
        private ActionPoints _actionPoints;
        private AbleToSkipTurn _ableToSkipTurn;
        
        public void Act()
        {
            var path = PathFinder.Instance.FindWayExcludeLastCell(_unit.currentCell, target.currentCell, false, false, false);

            if (path == null || path.Count == 0 || !_actionPoints.IsActionPointsEnough(_movementAbility.MovementCost))
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
            _actionPoints = _unit.actionPoints;
            _ableToSkipTurn = _unit.AbleToSkipTurn;
        }
    }
}
