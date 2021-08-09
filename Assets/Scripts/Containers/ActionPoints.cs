using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPoints : MonoBehaviour
{
    [SerializeField] private IntContainer _currentPoints;
    [SerializeField] private IntContainer _maxPointsCount;
    [SerializeField] private IntContainer _pointsRegeneration;
    [SerializeField] private AbleToSkipTurn _skipTurnModule;

    public UnityEvent OnActionPointsEnded;

    private void Start()
    {
        OnActionPointsEnded.AddListener(_skipTurnModule.AutoSkip);
    }

    public void RegeneratePoints()
    {
        _currentPoints.Content += _pointsRegeneration.Content;
        _currentPoints.Content = Mathf.Clamp(_currentPoints.Content, 0, _maxPointsCount.Content);
    }

    public bool CheckIsActionPointsEnough(int cost)
    {
        return _currentPoints.Content >= cost;
    }

    public void SpendPoints(int cost)
    {
        _currentPoints.Content -= cost;

        if (_currentPoints.Content <= 0)
            OnActionPointsEnded.Invoke();
    }
}
