using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostsActionPointsBehaviour : MonoBehaviour
{
    [SerializeField] private ActionPoints actionPoints;
    [SerializeField] private IntContainer defaultActionPointsCost;

    public bool IsActionPointsEnough()
    {
        return IsActionPointsEnough(defaultActionPointsCost.Content);
    }

    public bool IsActionPointsEnough(int actionPointsCost) 
    {
        return actionPoints.CheckIsActionPointsEnough(actionPointsCost);
    }

    public void SpendActionPoints()
    {
        actionPoints.SpendPoints(defaultActionPointsCost.Content);
    }

    public void SpendActionPoints(int actionPointsCost)
    {
        actionPoints.SpendPoints(actionPointsCost);
    }
}
