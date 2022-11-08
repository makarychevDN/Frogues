
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpearAttackAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private int defaultDamage;
        [SerializeField] private int criticalDamage;
        [SerializeField] private int radius;
        [SerializeField] private int cost;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private Grid _grid;
        private Cell _targetCell;
        private List<Cell> _attackArea;

        public void VisualizePreUse()
        {
            _attackArea = PathFinder.Instance.GetCellsAreaForAOE(_unit.currentCell, radius, true, false);
            _attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_attackArea));
            
            Vector3Int coordinate = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            try { _targetCell = Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y]; }
            catch (IndexOutOfRangeException e) { return; }
            
            if (!_attackArea.Contains(_targetCell))
                return;
            
            _targetCell.EnableSelectedCellHighlight(true);
        }

        public void Use()
        {
            if (!_attackArea.Contains(_targetCell))
                return;
            
            if(_targetCell.Content == null || _targetCell.Content.health == null)
                return;
            
            if(_unit.currentCell.DistanceToCell(_targetCell) == radius)
                _targetCell.Content.health.TakeDamage(criticalDamage);
            else
                _targetCell.Content.health.TakeDamage(defaultDamage);
        }
        
        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.actionPoints;
            _grid = unit.Grid;
            unit.AbilitiesManager.AddAbility(this);
        }

        [ContextMenu("Init")]
        public void Init() => Init(GetComponentInParent<Unit>());

        [ContextMenu("Set as current ability")]
        public void SetAsCurrentAbility()
        {
            var playersInput = _unit.input as PlayerInput;
            
            if (playersInput == null)
                return;

            playersInput.CurrentAbility = this;
        }
    }
}
