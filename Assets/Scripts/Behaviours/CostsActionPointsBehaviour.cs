using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostsActionPointsBehaviour : MonoBehaviour
{
    [SerializeField] private ActionPoints _actionPoints;
    [SerializeField] private int _defaultActionPointsCost;

    public bool IsActionPointsEnough()
    {
        return IsActionPointsEnough(_defaultActionPointsCost);
    }

    public bool IsActionPointsEnough(int actionPointsCost) 
    {
        return _actionPoints._currentPoints.Content >= actionPointsCost;
    }

    public void SpendActionPoints()
    {
        _actionPoints._currentPoints.Content -= _defaultActionPointsCost;
    }

    public void SpendActionPoints(int actionPointsCost)
    {
        _actionPoints._currentPoints.Content -= actionPointsCost;
    }
}
