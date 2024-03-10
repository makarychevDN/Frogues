using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AlternatesRunFromTargetAndDoSomethingAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] protected Unit target;
        protected Unit _unit;
        protected bool _moveFromTargetMode;

        public void Act()
        {
            if (_moveFromTargetMode)
            {
                TryToRunFromTarget();
            }
            else
            {
                TryToDoSomething();
            }
        }

        protected abstract void TryToDoSomething();

        protected virtual void TryToRunFromTarget()
        {
            if (_unit.MovementAbility == null || !_unit.MovementAbility.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            var theBestCellsToRetreat = CellsTaker.GetBestCellsToRetreatFromTarget(_unit, target);

            if (theBestCellsToRetreat.Contains(_unit.CurrentCell))
            {
                EndTurn();
                return;
            }

            _unit.MovementAbility.CalculateUsingArea();
            _unit.MovementAbility.UseOnCells(new List<Cell> { theBestCellsToRetreat.GetRandomElement() });
        }

        protected void EndTurn()
        {
            _unit.AbleToSkipTurn.AutoSkip();
            _moveFromTargetMode = !_moveFromTargetMode;
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}