using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class MovableAnimation : CurrentlyActiveBehaviour
{
    [SerializeField] private Transform _sprite;
    [SerializeField] private float _speedInUnitsPerSecond;

    [SerializeField] private float _jumpHeight;
    [SerializeField] private AnimationCurve _jumpCurve;

    private Cell _startCell, _targetCell;
    private float _currentTime, _totalTime, _distance;
    private bool _isPlaying;
    private const float _spriteAlignment = 0.35f;
    private Movable _movable;

    void Start()
    {
        _totalTime = _jumpCurve.keys[_jumpCurve.keys.Length - 1].time;
        _movable = GetComponent<Movable>();
    }

    public void Play(Cell startCell, Cell targetCell)
    {
        _isPlaying = true;
        _startCell = startCell;
        _targetCell = targetCell;
        ActiveNow = true;
        _distance = Vector3.Distance(startCell.transform.position, targetCell.transform.position);
    }

    void Update()
    {
        if (!_isPlaying)
            return;

        _sprite.position = Vector3.Lerp(_startCell.transform.position, _targetCell.transform.position, _currentTime);
        _sprite.position += Vector3.up * _spriteAlignment;
        _sprite.position += Vector3.up * _jumpCurve.Evaluate(_currentTime) * _jumpHeight;
        TimerStep();
    }

    public void TimerStep()
    {
        _currentTime += Time.deltaTime * _speedInUnitsPerSecond/ _distance;
        if (_currentTime > _totalTime)
        {
            _currentTime = 0;
            _isPlaying = false;
            ActiveNow = false;
            _sprite.localPosition = Vector3.zero;
            _movable.StopMovement(_targetCell);
        }
    }
}