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
        private const float defaultDiagonalAngle = 41.18592f;
        private Transform _spriteParent, _shadow;
        private SpriteRotator _spriteRotator;

        #region Setters
        
        public Unit Unit { set => _unit = value; }
        public ActionPoints ActionPoints { set => _actionPoints = value; }

        public Transform SpriteParent
        {
            set
            {
                _spriteParent = value;
                _spriteAlignment = _spriteParent.parent.localPosition.y;
            }
        }

        public Transform Shadow
        {
            set
            {
                _shadow = value;
                _shadowAlignment = _shadow.parent.localPosition.y;
            }
        }

        public SpriteRotator SpriteRotator { set => _spriteRotator = value; }

        #endregion

        private void Awake()
        {
            _totalTime = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
        }

        public void Move(Cell targetCell) => Move(targetCell, true);

        public void Move(Cell targetCell, bool startCellBecomeEmptyOnMove) =>
            Move(targetCell, defaultMovementCost, defaultSpeed, defaultJumpHeight, true);

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

            Play(_unit.currentCell, targetCell, speed, jumpHeight);
            _unit.currentCell = null;
            OnMovementStart.Invoke();
        }

        private void StopMovement(Cell targetCell)
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
        
        private void Play(Cell startCell, Cell targetCell) =>
            Play(startCell, targetCell, defaultSpeed, defaultJumpHeight);
        

        private void Play(Cell startCell, Cell targetCell, float speed, float jumpHeight)
        {
            var horizonVector = startCell.transform.position.x < targetCell.transform.position.x
                ? Vector3.right
                : Vector3.left;

            var angleAnimSpeedModificator = Mathf.Sin(Vector2.Angle(
                (targetCell.transform.position - startCell.transform.position).normalized,
                (startCell.transform.position + horizonVector - startCell.transform.position).normalized));
            
            _speed = speed + speed * angleAnimSpeedModificator;
            _speed = Mathf.Clamp(_speed,speed + speed * Mathf.Sin(defaultDiagonalAngle), 2 * _speed);
            _jumpHeight = jumpHeight;
            _isPlaying = true;
            _startCell = startCell;
            _targetCell = targetCell;
            CurrentlyActiveObjects.Add(this);
            _distance = Vector3.Distance(startCell.transform.position, targetCell.transform.position);

            if (startCell.transform.position.x < targetCell.transform.position.x)
                _spriteRotator.TurnRight();
            else
                _spriteRotator.TurnLeft();
        }

        void Update()
        {
            if (!_isPlaying)
                return;

            var lerpPosition =
                Vector3.Lerp(_startCell.transform.position, _targetCell.transform.position, _currentTime);
            float scaledShadowSize = 0;

            _spriteParent.position = lerpPosition;
            _spriteParent.position += Vector3.up * _spriteAlignment;
            _spriteParent.position += Vector3.up * (jumpCurve.Evaluate(_currentTime) * _jumpHeight);

            _shadow.position = lerpPosition;
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