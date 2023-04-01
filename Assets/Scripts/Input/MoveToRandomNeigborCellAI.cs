using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework 
{ 
    public class MoveToRandomNeigborCellAI : MonoBehaviour, IAbleToAct
    {
        private Unit _unit;
        private MovementAbility _movementAbility;
        private ActionPoints _actionPoints;
        private AbleToSkipTurn _ableToSkipTurn;
        private bool _movedAlready;

        public void Act()
        {
            var possibleToMovementCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_unit.CurrentCell, 1, true, false);
            if (possibleToMovementCells == null || possibleToMovementCells.Count == 0 || !_actionPoints.IsActionPointsEnough(_movementAbility.GetCost()))
            {
                _ableToSkipTurn.AutoSkip();
                return;
            }

            _movementAbility.TargetCell = possibleToMovementCells.EmptyCellsOnly().GetRandomElement();
            _movementAbility.Use();
            _movedAlready = true;
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _movementAbility = _unit.MovementAbility;
            _actionPoints = _unit.ActionPoints;
            _ableToSkipTurn = _unit.AbleToSkipTurn;
            _ableToSkipTurn.OnSkipTurn.AddListener(ClearTheFlag);
        }

        private void ClearTheFlag() => _movedAlready = false;
    }
}