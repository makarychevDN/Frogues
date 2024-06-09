using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AlternatesRunFromTargetAndDoSomethingAI : MonoBehaviour, IAbleToAct
    {
        protected Unit _target;
        protected Unit _owner;
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
            _target = _owner.CurrentRoom.PlayableCharacters[0];

            if (_owner.MovementAbility == null || !_owner.MovementAbility.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            var theBestCellsToRetreat = CellsTaker.GetBestCellsToRetreatFromTarget(_owner, _target, _owner.CurrentRoom);

            if (theBestCellsToRetreat.Contains(_owner.CurrentCell))
            {
                EndTurn();
                return;
            }

            _owner.MovementAbility.CalculateUsingArea();
            _owner.MovementAbility.UseOnCells(new List<Cell> { theBestCellsToRetreat.GetRandomElement() });
        }

        protected void EndTurn()
        {
            _owner.AbleToSkipTurn.AutoSkip();
            _moveFromTargetMode = !_moveFromTargetMode;
        }

        public void Init(Unit owner)
        {
            _owner = owner;
        }
    }
}