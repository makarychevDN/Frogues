using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class MovementAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private bool findTargetByMouse;
        private Unit _unit;
        private Movable _movable;
        private ActionPoints _actionPoints;
        private List<Cell> _path = new();
        private List<Cell> _prePath = new();
        private Cell _targetCell;
        private List<Cell> _movementArea = new();
        private List<Cell> _preMovementArea = new();

        public Cell TargetCell
        {
            get => _targetCell;
            set => _targetCell = value;
        }

        public void VisualizePreUse()
        {
            CalculateMovementAreaAndPath(ref _preMovementArea, ref _prePath);
            _preMovementArea.ForEach(cell => cell.EnableValidForMovementCellHighlight(_preMovementArea));

            if(_prePath != null && _prePath.Count != 0)
                _prePath.Insert(0, _unit.CurrentCell);
            
            for (int i = 1; i < _prePath.Count - 1; i++)
            {
                _prePath[i].EnableTrail(
                    (_prePath[i - 1].transform.position - _prePath[i].transform.position).normalized.ToVector2());
                _prePath[i].EnableTrail(
                    (_prePath[i + 1].transform.position - _prePath[i].transform.position).normalized.ToVector2());
            }

            if(_prePath == null || _prePath.Count == 0)
                return;

            _targetCell.EnablePathDot(true);
            _prePath[0].EnableTrail((_prePath[1].transform.position - _prePath[0].transform.position).normalized.ToVector2());
            _prePath[_prePath.Count - 1]
                .EnableTrail((_prePath[_prePath.Count - 2].transform.position - _prePath[_prePath.Count - 1].transform.position)
                    .normalized.ToVector2());

            _actionPoints.PreSpendPoints((_prePath.Count - 1) * _movable.DefaultMovementCost);
        }

        private void CalculateMovementAreaAndPath(ref List<Cell> movementArea, ref List<Cell> path)
        {
            path.Clear();

            movementArea = Room.Instance.PathFinder.GetCellsAreaByActionPoints(_unit.CurrentCell,
                _actionPoints.CurrentActionPoints,
                _unit.Movable.DefaultMovementCost, false, false, true);

            if (findTargetByMouse)
                _targetCell = CellsTaker.TakeCellByMouseRaycast();
            

            if(_targetCell == null || !movementArea.Contains(_targetCell))
                return;

            path = Room.Instance.PathFinder.FindWay(_unit.CurrentCell, _targetCell, false,
                false, true);
        }

        public void Use() => ApplyEffect();

        public void ApplyEffect()
        {
            CalculateMovementAreaAndPath(ref _movementArea, ref _path);
        }

        public void Init(Unit unit)
        {
            _unit = unit;
            _movable = unit.Movable;
            _actionPoints = unit.ActionPoints;
        }

        public int GetCost() => _movable.DefaultMovementCost;

        public bool IsPartOfWeapon() => false;

        private void Update()
        {
            if (!CurrentlyActiveObjects.SomethingIsActNow && _path.Count != 0)
            {
                _movable.Move(_path[0]);
                _path.RemoveAt(0);
            }
        }
    }
}