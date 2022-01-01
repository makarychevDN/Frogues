using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAndBumpIntoTargetAI : BaseInput
{
    [SerializeField] private UnitContainer targetContainer;
    private List<Cell> _pathToTarget;

    private void Start()
    {
        unit.GetComponentInChildren<ActionPoints>().OnActionPointsEnded.AddListener(ClearPath);
    }

    public override void Act()
    {
        if (_pathToTarget == null)
            _pathToTarget = PathFinder.Instance.FindWay(unit.currentCell, targetContainer.Content.currentCell);

        unit.movable.Move(_pathToTarget[0]);
        if (_pathToTarget != null)
            _pathToTarget.RemoveAt(0);
    }

    public void ClearPath()
    {
        _pathToTarget = null;
    }
}
