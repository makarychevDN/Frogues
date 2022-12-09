using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikedBall : MonoBehaviour, IAbility, IAbleToUseOnTarget, IAbleToDrawAbilityButton
    {
        [SerializeField] private int cost;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        [SerializeField] private AudioSource impactSound;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private Grid _grid;
        private Cell _targetCell;
        private List<Cell> _attackArea;
        private SpriteRotator _spriteRotator;
        private Movable _movable;
        private Cell _endOfPathCell;
        private Cell _startOfPathCell;
        private HexDir _directionToAttack;

        public void VisualizePreUse()
        {
            _endOfPathCell = null;
            _startOfPathCell = null;
            _attackArea = CellsTaker.TakeCellsLinesInAllDirections(_unit.CurrentCell);
            _attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_attackArea));
            
            Vector3Int coordinate = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            try { _targetCell = Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y]; }
            catch (IndexOutOfRangeException e) { return; }

            if (!_attackArea.Contains(_targetCell))
                return;

            List<Cell> targetCells = CellsTaker.TakeCellsLineWhichContainCell(_unit.CurrentCell, _targetCell);
            targetCells.ForEach(cell => cell.EnableSelectedCellHighlight(true));
            _actionPoints.PreTakenCurrentPoints -= cost;

            _endOfPathCell = targetCells[targetCells.Count - 1];
            _startOfPathCell = targetCells[0];
            _directionToAttack = _unit.CurrentCell.CellNeighbours.GetHexDirByNeighbor(_startOfPathCell);
            var cellToApplyEffect = _targetCell.CellNeighbours.GetNeighborByHexDir(_directionToAttack);
            
            if(!cellToApplyEffect.IsEmpty && cellToApplyEffect.Content.Health != null)
                cellToApplyEffect.Content.Health.PreTakeDamage(1);
        }

        public void Use()
        {
            if(_endOfPathCell == null)
                return;
            
            _movable.Move(_endOfPathCell, 0, 40, 0.6f);
        }

        public void ApplyEffect()
        {
            
        }

        private void RemoveFromCurrentlyActiveList() => CurrentlyActiveObjects.Remove(this);

        public void Init(Unit unit)
        {
            _unit = unit;
            unit.AbilitiesManager.AddAbility(this);
            _grid = unit.Grid;
            _actionPoints = unit.ActionPoints;
            _movable = unit.Movable;
        }

        public int GetCost() => cost;

        public bool PossibleToUseOnTarget(Unit target)
        {
            return false;
        }
        
        public void UseOnTarget(Unit target)
        {
            _targetCell = target.CurrentCell;
            Use();
        }

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
    }
}
