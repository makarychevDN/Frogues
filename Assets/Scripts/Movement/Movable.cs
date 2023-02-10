using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Movable : MonoBehaviour
    {
        [SerializeField] private bool canBumpIntoUnit;
        [SerializeField] private int defaultMovementCost;
        [Range(0.1f, 30), SerializeField] private float defaultSpeed;
        [SerializeField] private float defaultJumpHeight;
        [SerializeField] private AnimationCurve jumpCurve;

        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementEnd;
        public UnityEvent OnBumpInto;
        public UnityEvent<List<Cell>> OnBumpIntoCell;

        private Unit _unit;
        private ActionPoints _actionPoints;
        private Cell _startCell, _targetCell;
        private float _currentTime, _totalTime, _distance;
        private bool _isPlaying;
        private float _spriteAlignment, _shadowAlignment;
        private float _jumpHeight, _speed;
        private Transform _spriteParent, _shadow;
        private SpriteRotator _spriteRotator;

        public int DefaultMovementCost => defaultMovementCost;

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.ActionPoints;
            _spriteParent = unit.SpriteParent;
            _spriteAlignment = _spriteParent.parent.localPosition.y;
            _shadow = _unit.Shadow;
            _shadowAlignment = _shadow.parent.transform.localPosition.y;
            _spriteRotator = _unit.SpriteRotator;
        }

        private void Awake()
        {
            _totalTime = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
        }

        public void Move(Cell targetCell) => Move(targetCell, true);

        public void Move(Cell targetCell, bool startCellBecomeEmptyOnMove) =>
            Move(targetCell, defaultMovementCost, defaultSpeed, defaultJumpHeight, true, true);

        public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight) =>
            Move(targetCell, movementCost, speed, jumpHeight, true, true);
        
        public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight, bool needToRotateSprite) =>
            Move(targetCell, movementCost, speed, jumpHeight, true, needToRotateSprite);

        public void Move(Cell targetCell, int movementCost, float speed, float jumpHeight,
            bool startCellBecomeEmptyOnMove, bool needToRotateSprite)
        {

            if (!targetCell.IsEmpty && !(canBumpIntoUnit || targetCell.Content.Small) || !_actionPoints.IsActionPointsEnough(movementCost))
                return;

            _actionPoints.SpendPoints(movementCost);
            targetCell.chosenToMovement = true;

            if (startCellBecomeEmptyOnMove)
                _unit.CurrentCell.Content = null;

            Play(_unit.CurrentCell, targetCell, speed, jumpHeight, needToRotateSprite);
            _unit.CurrentCell = null;
            OnMovementStart.Invoke();
        }

        private void StopMovement(Cell targetCell)
        {
            targetCell.chosenToMovement = false;
            _unit.transform.position = targetCell.transform.position;

            if (!targetCell.IsEmpty)
            {
                if (!_unit.Small && targetCell.Content.Small)
                {
                    var unitToStepOnIt = targetCell.Content;
                    targetCell.Content = _unit;
                    unitToStepOnIt.CurrentCell = null;
                    _unit.CurrentCell = targetCell;
                    //unitToStepOnIt.stepOnUnitTrigger.Run(_unit);
                    return;
                }

                OnBumpInto.Invoke();
                OnBumpIntoCell.Invoke(new List<Cell>() {targetCell});
                return;
            }

            targetCell.Content = _unit;
            OnMovementEnd.Invoke();
        }
        
        private void Play(Cell startCell, Cell targetCell) =>
            Play(startCell, targetCell, defaultSpeed, defaultJumpHeight);
        

        private void Play(Cell startCell, Cell targetCell, float speed, float jumpHeight, bool needToRotateSprite = true)
        {
            _speed = speed;
            _jumpHeight = jumpHeight;
            _isPlaying = true;
            _startCell = startCell;
            _targetCell = targetCell;
            CurrentlyActiveObjects.Add(this);
            _distance = Vector3.Distance(startCell.transform.position, targetCell.transform.position);
            
            if(needToRotateSprite)
                _spriteRotator.TurnAroundByTarget(targetCell.transform.position);
        }

        void Update()
        {
            if (!_isPlaying)
                return;
            
            _spriteParent.position =
                PositionOnCurveCalculator.Calculate(_startCell, _targetCell, jumpCurve, _currentTime, _jumpHeight);
            _spriteParent.position += Vector3.up * _spriteAlignment;

            
            float scaledShadowSize = 0;
            _shadow.position =
                PositionOnCurveCalculator.Calculate(_startCell, _targetCell, jumpCurve, _currentTime, 0);
            _shadow.position += Vector3.up * _shadowAlignment;
            scaledShadowSize = Mathf.Clamp(1 - jumpCurve.Evaluate(_currentTime) * _jumpHeight, 0, 1);
            _shadow.localScale = new Vector3(scaledShadowSize, scaledShadowSize, 0);
            
            TimerStep();
        }

        private void TimerStep()
        {
            _currentTime += Time.deltaTime * _speed / _distance;
            if (_currentTime >= _totalTime)
            {
                _currentTime = 0;
                _isPlaying = false;
                CurrentlyActiveObjects.Remove(this);
                _spriteParent.localPosition = Vector3.zero;
                _shadow.localPosition = Vector3.zero;
                StopMovement(_targetCell);
            }
        }
    }
}