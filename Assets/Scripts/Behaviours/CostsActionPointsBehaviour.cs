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
        return _actionPoints.CheckIsActionPointsEnough(actionPointsCost);
    }

    public void SpendActionPoints()
    {
        _actionPoints.SpendPoints(_defaultActionPointsCost);
    }

    public void SpendActionPoints(int actionPointsCost)
    {
        _actionPoints.SpendPoints(actionPointsCost);
    }
}
