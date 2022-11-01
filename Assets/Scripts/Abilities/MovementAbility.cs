using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class MovementAbility : MonoBehaviour, IAbility
    {
        private Unit _unit;
        private Movable _movable;
        private ActionPoints _actionPoints;
        private Grid _grid;
        private List<Cell> _path = new();
        private List<Cell> _prePath = new();

        #region GetSet
        
        public Unit Unit
        {
            get => _unit;
            set => _unit = value;
        }

        public Movable Movable
        {
            get => _movable;
            set => _movable = value;
        }

        public ActionPoints ActionPoints
        {
            get => _actionPoints;
            set => _actionPoints = value;
        }

        public Grid Grid
        {
            get => _grid;
            set => _grid = value;
        }

        #endregion
        
        public void VisualizePreUse()
        {
            _prePath.Clear();
            
            if(_path.Count != 0)
                return;
            
            var movementArea = PathFinder.Instance.GetCellsAreaByActionPoints(_unit.currentCell,
                _actionPoints.CurrentActionPoints,
                _unit.movable.DefaultMovementCost, false, false, true);
            movementArea.ForEach(cell => cell.EnableValidForMovementCellHighlight(movementArea));
            
            Vector3Int coordinate = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Cell targetCell;
            
            try { targetCell = Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y]; }
            catch (IndexOutOfRangeException e) { return; }

            if (!movementArea.Contains(targetCell))
                return;

            targetCell.EnablePathDot(true);
            _prePath = PathFinder.Instance.FindWay(_unit.currentCell, targetCell, false,
                false, true);
            
            _prePath.Insert(0, _unit.currentCell);
            _prePath.GetLast().EnablePathDot(true);

            if (_prePath.Count == 1)
                return;

            for (int i = 1; i < _prePath.Count - 1; i++)
            {
                _prePath[i].EnableTrail(
                    (_prePath[i - 1].transform.position - _prePath[i].transform.position).normalized.ToVector2());
                _prePath[i].EnableTrail(
                    (_prePath[i + 1].transform.position - _prePath[i].transform.position).normalized.ToVector2());
            }

            _prePath[0].EnableTrail((_prePath[1].transform.position - _prePath[0].transform.position).normalized.ToVector2());
            _prePath[_prePath.Count - 1]
                .EnableTrail((_prePath[_prePath.Count - 2].transform.position - _prePath[_prePath.Count - 1].transform.position)
                    .normalized.ToVector2());

            _actionPoints.PreTakenCurrentPoints -= _prePath.Count - 1;
        }

        public void Use()
        {
            _path = new List<Cell>(_prePath);
        }

        private void Update()
        {
            if (!CurrentlyActiveObjects.SomethingIsActNow && _path.Count != 0)
            {
                _unit.movable.Move(_path[0]);
                _path.RemoveAt(0);
            }
        }
    }
}