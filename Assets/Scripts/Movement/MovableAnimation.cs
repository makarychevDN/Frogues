using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class MovableAnimation : CurrentlyActiveBehaviour
{
    [SerializeField] private Transform sprite;
    [Range(0, 30), SerializeField] private float speedInUnitsPerSecond;
    [SerializeField] private float jumpHeight;
    [SerializeField] private AnimationCurve jumpCurve;
    private Cell _startCell, _targetCell;
    private float _currentTime, _totalTime, _distance;
    private bool _isPlaying;
    private float _spriteAlignment;
    private Movable _movable;

    void Start()
    {
        _spriteAlignment = sprite.parent.localPosition.y;
        _totalTime = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
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

        sprite.position = Vector3.Lerp(_startCell.transform.position, _targetCell.transform.position, _currentTime);
        sprite.position += Vector3.up * _spriteAlignment;
        sprite.position += Vector3.up * jumpCurve.Evaluate(_currentTime) * jumpHeight;
        TimerStep();
    }

    public void TimerStep()
    {
        _currentTime += Time.deltaTime * speedInUnitsPerSecond/ _distance;
        if (_currentTime > _totalTime)
        {
            _currentTime = 0;
            _isPlaying = false;
            ActiveNow = false;
            sprite.localPosition = Vector3.zero;
            _movable.StopMovement(_targetCell);
        }
    }
}
