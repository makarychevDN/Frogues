using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FollowAndAttackTargetAI : BaseInput
{
    [SerializeField] private Unit target;
    [SerializeField] private Weapon activeWeapon;
    [SerializeField] private AbleToSkipTurn skipTurnModule;
    private List<Cell> _pathToTarget;

    private void Start()
    {
        unit.GetComponentInChildren<ActionPoints>().OnActionPointsEnded.AddListener(ClearPath);
    }

    public override void Act()
    {
        if (_pathToTarget == null)
            _pathToTarget = PathFinder.Instance.FindWay(unit.currentCell, target.currentCell);

        if (activeWeapon.PossibleToHitExpectedTarget)
        {
            activeWeapon.Use();
            return;
        }

        if(_pathToTarget.Count > 1)
        {
            unit.movable.Move(_pathToTarget[0]);
            if(_pathToTarget != null) _pathToTarget.RemoveAt(0);
            return;
        }

        skipTurnModule.AutoSkip();
        ClearPath();
    }

    public void ClearPath()
    {
        _pathToTarget = null;
    }
}
