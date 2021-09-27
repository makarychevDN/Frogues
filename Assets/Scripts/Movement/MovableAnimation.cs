using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class MovableAnimation : CurrentlyActiveBehaviour
{
    [SerializeField] private Transform spriteParent;
    [SerializeField] private Transform shadow;
    [Range(0.1f, 30), SerializeField] private float defaultSpeed;
    [SerializeField] private float defaultJumpHeight;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private SpriteRotator spriteRotator;
    private Cell _startCell, _targetCell;
    private float _currentTime, _totalTime, _distance;
    private bool _isPlaying;
    private float _spriteAlignment, _shadowAlignment;
    private Movable _movable;
    private float _jumpHeight, _speed;

    void Start()
    {
        _spriteAlignment = spriteParent.parent.localPosition.y;
        _shadowAlignment = shadow.parent.localPosition.y;
        _totalTime = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
        _movable = GetComponent<Movable>();
    }

    public void Play(Cell startCell, Cell targetCell)
    {
        Play(startCell, targetCell, defaultSpeed, defaultJumpHeight);
    }

    public void Play(Cell startCell, Cell targetCell, float speed, float jumpHeight)
    {
        _speed = speed;
        _jumpHeight = jumpHeight;
        _isPlaying = true;
        _startCell = startCell;
        _targetCell = targetCell;
        ActiveNow = true;
        _distance = Vector3.Distance(startCell.transform.position, targetCell.transform.position);

        if (startCell.transform.position.x < targetCell.transform.position.x)
            spriteRotator.TurnRight();
        else
            spriteRotator.TurnLeft();
    }

    void Update()
    {
        if (!_isPlaying)
            return;

        var lerpPosition = Vector3.Lerp(_startCell.transform.position, _targetCell.transform.position, _currentTime);
        float scaledShadowSize = 0;

        spriteParent.position = lerpPosition;
        spriteParent.position += Vector3.up * _spriteAlignment;
        spriteParent.position += Vector3.up * jumpCurve.Evaluate(_currentTime) * _jumpHeight;

        shadow.position = lerpPosition;
        shadow.position += Vector3.up * _shadowAlignment;
        scaledShadowSize = Mathf.Clamp(1 - jumpCurve.Evaluate(_currentTime) * _jumpHeight, 0, 1);
        shadow.localScale = new Vector3(scaledShadowSize, scaledShadowSize, 0);
        TimerStep();
    }

    public void TimerStep()
    {
        _currentTime += Time.deltaTime * _speed / _distance;
        if (_currentTime > _totalTime)
        {
            _currentTime = 0;
            _isPlaying = false;
            ActiveNow = false;
            spriteParent.localPosition = Vector3.zero;
            shadow.localPosition = Vector3.zero;
            _movable.StopMovement(_targetCell);
        }
    }
}
