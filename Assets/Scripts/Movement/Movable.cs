using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    [RequireComponent(typeof(MovableAnimation))]
    public class Movable : MonoBehaviour
    {
        [SerializeField] private bool canBumpIntoUnit;
        [SerializeField] private int defaultMovementCost;
        
        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementEnd;
        public UnityEvent OnBumpInto;
        public UnityEvent<List<Cell>> OnBumpIntoCell;

        private Unit _unit;
        private MovableAnimation _movableAnimation;
        private ActionPoints _actionPoints;

        public Unit Unit { set => _unit = value; }
        public ActionPoints ActionPoints { set => _actionPoints = value; }

        private void Awake()
        {
            _movableAnimation = GetComponent<MovableAnimation>();
        }

        public void Move(Cell targetCell) => Move(targetCell, true);

        public void Move(Cell targetCell, bool startCellBecomeEmptyOnMove)
        {
            if (!targetCell.IsEmpty && !(canBumpIntoUnit || targetCell.Content.small) || !_actionPoints.CheckIsActionPointsEnough(defaultMovementCost))
                return;

            _actionPoints.SpendPoints(defaultMovementCost);
            targetCell.chosenToMovement = true;

            if (startCellBecomeEmptyOnMove)
                _unit.currentCell.Content = null;

            _movableAnimation.Play(_unit.currentCell, targetCell);
            _unit.currentCell = null;
            OnMovementStart.Invoke();
        }

        public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight) =>
            Move(targetCell, movementCost, speed, jumpHeight, true);

        public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight,
            bool startCellBecomeEmptyOnMove)
        {

            if (!targetCell.IsEmpty && !(canBumpIntoUnit || targetCell.Content.small) || !_actionPoints.CheckIsActionPointsEnough(movementCost))
                return;

            _actionPoints.SpendPoints(movementCost);
            targetCell.chosenToMovement = true;

            if (startCellBecomeEmptyOnMove)
                _unit.currentCell.Content = null;

            _movableAnimation.Play(_unit.currentCell, targetCell, speed, jumpHeight);
            _unit.currentCell = null;
            OnMovementStart.Invoke();
        }

        public void StopMovement(Cell targetCell)
        {
            targetCell.chosenToMovement = false;
            _unit.transform.position = targetCell.transform.position;

            if (!targetCell.IsEmpty)
            {
                if (!_unit.small && targetCell.Content.small)
                {
                    var unitToStepOnIt = targetCell.Content;
                    targetCell.Content = _unit;
                    unitToStepOnIt.currentCell = null;
                    _unit.currentCell = targetCell;
                    unitToStepOnIt.stepOnUnitTrigger.Run(_unit);
                    return;
                }

                OnBumpInto.Invoke();
                OnBumpIntoCell.Invoke(new List<Cell>() {targetCell});
                return;
            }

            targetCell.Content = _unit;
            OnMovementEnd.Invoke();
        }
    }
}