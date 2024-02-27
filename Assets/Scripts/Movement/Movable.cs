using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Movable : MonoBehaviour
    {
        [SerializeField] private bool canBumpIntoUnit;
        [Range(0.1f, 30), SerializeField] private float defaultSpeed;
        [SerializeField] private float defaultJumpHeight;
        [SerializeField] private AnimationCurve jumpCurve;

        public UnityEvent OnMovementStart;
        public UnityEvent<Cell> OnMovementStartFromCell;
        public UnityEvent OnMovementEnd;
        public UnityEvent<Cell> OnMovementEndOnCell;
        public UnityEvent OnBumpInto;
        public UnityEvent<Unit> OnBumpIntoUnit;

        private Unit _unit;
        private Vector3 _startCellPosition, _targetCellPosition;
        private Cell _targetCell;
        private float _currentTime, _totalTime, _distance;
        private bool _isPlaying;
        private float _spriteAlignment, _shadowAlignment;
        private float _jumpHeight, _speed;
        private Transform _spriteParent, _shadow;
        private SpriteRotator _spriteRotator;

        public bool CanBumpIntoUnit => canBumpIntoUnit;

        public void Init(Unit unit)
        {
            _unit = unit;
            _spriteParent = unit.SpriteParent;
            _spriteAlignment = _spriteParent.parent.localPosition.y;
            _shadow = _unit.Shadow;
            _shadowAlignment = _shadow.parent.transform.localPosition.y;
            _spriteRotator = _unit.SpriteRotator;
            _totalTime = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
            OnBumpInto.AddListener(unit.Health.DieFromBumpInto);
        }

        public bool IsPossibleToMoveOnCell(Cell targetCell)
        {
            if (targetCell?.Content == _unit)
                return false;

            return targetCell.IsEmpty || canBumpIntoUnit || targetCell.Content.Small;
        }

        public void Move(Cell targetCell, bool startCellBecomeEmptyOnMove = true, bool needToRotateSprite = true, bool needToModificateJumpHeightByDistance = true) =>
            Move(targetCell, defaultSpeed, defaultJumpHeight, startCellBecomeEmptyOnMove,
                needToRotateSprite);

        public void Move(Cell targetCell, float speed, float jumpHeight,
            bool startCellBecomeEmptyOnMove = true, bool needToRotateSprite = true, bool needToModificateJumpHeightByDistance = true)
        {
            if (!IsPossibleToMoveOnCell(targetCell))
                return;

            targetCell.chosenToMovement = true;

            if (startCellBecomeEmptyOnMove)
                _unit.CurrentCell.Content = null;

            Play(_unit.CurrentCell, targetCell, speed, jumpHeight, needToRotateSprite, needToModificateJumpHeightByDistance);
            OnMovementStart.Invoke();
            OnMovementStartFromCell.Invoke(_unit.CurrentCell);
            _unit.CurrentCell = null;
        }

        private void StopMovement(Cell targetCell)
        {
            targetCell.chosenToMovement = false;
            _unit.transform.position = targetCell.transform.position;

            if (!targetCell.IsEmpty)
            {
                if (!_unit.Small && targetCell.Content.Small)
                {
                    targetCell.Content.OnStepOnThisUnit.Invoke();
                    targetCell.Content.OnStepOnThisUnitByUnit.Invoke(_unit);
                }
                else
                {
                    OnBumpInto.Invoke();
                    OnBumpIntoUnit.Invoke(targetCell.Content);
                    InvokeOnMovementEndEvents(targetCell);
                    return;
                }
            }

            _unit.CurrentCell = targetCell;
            targetCell.Content = _unit;
            InvokeOnMovementEndEvents(_unit.CurrentCell);
        }

        private void InvokeOnMovementEndEvents(Cell cell)
        {
            OnMovementEnd.Invoke();
            OnMovementEndOnCell.Invoke(cell);
        }

        private void Play(Cell startCell, Cell targetCell, float speed, float jumpHeight, bool needToRotateSprite = true, bool needToModificateJumpHeightByDistance = true)
        {
            _speed = speed;
            _jumpHeight = jumpHeight;
            if(needToModificateJumpHeightByDistance)
                _jumpHeight *= startCell.DistanceToCell(targetCell);
            _isPlaying = true;
            _targetCell = targetCell;
            _startCellPosition = startCell == null ? transform.position : startCell.transform.position;
            _targetCellPosition = targetCell.transform.position;
            CurrentlyActiveObjects.Add(this);
            _distance = Vector3.Distance(_startCellPosition, _targetCellPosition);
            
            if(needToRotateSprite)
                _spriteRotator.TurnAroundByTarget(targetCell.transform.position);
        }

        void Update()
        {
            if (!_isPlaying)
                return;
            
            _spriteParent.position =
                PositionOnCurveCalculator.Calculate(_startCellPosition, _targetCellPosition, jumpCurve, _currentTime, _jumpHeight);
            _spriteParent.position += Vector3.up * _spriteAlignment;

            
            float scaledShadowSize = 0;
            _shadow.position =
                PositionOnCurveCalculator.Calculate(_startCellPosition, _targetCellPosition, jumpCurve, _currentTime, 0);
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