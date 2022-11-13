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
        private Grid _grid;
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

        public int MovementCost => _movable.DefaultMovementCost;

        public void VisualizePreUse()
        {
            CalculateMovementAreaAndPath(ref _preMovementArea, ref _prePath);
            
            _preMovementArea.ForEach(cell => cell.EnableValidForMovementCellHighlight(_preMovementArea));
            
            if(_prePath != null && _prePath.Count != 0)
                _prePath.Insert(0, _unit.currentCell);
            
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

            _actionPoints.PreTakenCurrentPoints -= (_prePath.Count - 1) * _movable.DefaultMovementCost;
        }

        private void CalculateMovementAreaAndPath(ref List<Cell> movementArea, ref List<Cell> path)
        {
            path.Clear();

            movementArea = PathFinder.Instance.GetCellsAreaByActionPoints(_unit.currentCell,
                _actionPoints.CurrentActionPoints,
                _unit.movable.DefaultMovementCost, false, false, true);

            if (findTargetByMouse)
            {
                Vector3Int coordinate = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                try { _targetCell = Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y]; }
                catch (IndexOutOfRangeException e) { return; }
            }

            if(!movementArea.Contains(_targetCell))
                return;

            path = PathFinder.Instance.FindWay(_unit.currentCell, _targetCell, false,
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
            _grid = unit.Grid;
            _movable = unit.movable;
            _actionPoints = unit.actionPoints;
        }

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