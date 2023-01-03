using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpikedBall : MonoBehaviour, IAbility, IAbleToUseOnTarget, IAbleToDrawAbilityButton
    {
        [SerializeField] private int damage;
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
        private Cell _cellToApplyEffect;
        private HexDir _directionToAttack;

        public void VisualizePreUse()
        {
            _endOfPathCell = null;
            _startOfPathCell = null;
            _cellToApplyEffect = null;
            _attackArea = CellsTaker.TakeCellsLinesInAllDirections(_unit.CurrentCell);
            _attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_attackArea));

            _targetCell = CellsTaker.TakeCellByMouseRaycast();

            if (!_attackArea.Contains(_targetCell))
                return;

            List<Cell> targetCells = CellsTaker.TakeCellsLineWhichContainCell(_unit.CurrentCell, _targetCell);
            targetCells.ForEach(cell => cell.EnableSelectedCellHighlight(true));
            _actionPoints.PreSpendPoints(cost);

            _endOfPathCell = targetCells[targetCells.Count - 1];
            _startOfPathCell = targetCells[0];
            _directionToAttack = _unit.CurrentCell.CellNeighbours.GetHexDirByNeighbor(_startOfPathCell);
            _cellToApplyEffect = _endOfPathCell.CellNeighbours.GetNeighborByHexDir(_directionToAttack);
            
            if(!_cellToApplyEffect.IsEmpty && _cellToApplyEffect.Content.Health != null)
                _cellToApplyEffect.Content.Health.PreTakeDamage(damage);
        }

        public void Use()
        {
            if(_endOfPathCell == null)
                return;
            
            _movable.OnMovementEnd.AddListener(ApplyEffect);
            _movable.Move(_endOfPathCell, 0, 30, 0.6f);
            impactSound.Play();
        }

        public void ApplyEffect()
        {
            if (!_cellToApplyEffect.IsEmpty && _cellToApplyEffect.Content.Health != null)
            {
                _cellToApplyEffect.Content.Health.TakeDamage(damage);
            }

            _movable.OnMovementEnd.RemoveListener(ApplyEffect);
        }

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
