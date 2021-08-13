using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FollowAndAttackTargetAI : BaseInput
{
    [SerializeField] private Unit _target;
    private List<Cell> _pathToTarget;

    public override void Act()
    {
        if (_pathToTarget == null)
            _pathToTarget = PathFinder.Instance.FindWay(_unit._currentCell, _target._currentCell);

        _unit._movable.Move(_pathToTarget[0]);
        if(_pathToTarget != null)
            _pathToTarget.RemoveAt(0);
    }

    public void ClearPath()
    {
        _pathToTarget = null;
    }
}
