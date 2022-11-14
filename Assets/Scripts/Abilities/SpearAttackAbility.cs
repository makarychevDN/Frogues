
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpearAttackAbility : MonoBehaviour, IAbility, IAbleToUseOnTarget, IAbleToDrawAbilityButton
    {
        [SerializeField] private bool findTargetByMouse;
        [SerializeField] private int defaultDamage;
        [SerializeField] private int criticalDamage;
        [SerializeField] private int radius;
        [SerializeField] private int cost;
        [SerializeField] private float animationBeforeImpactTime;
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        [SerializeField] private AudioSource impactSound;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private Grid _grid;
        private Cell _targetCell;
        private List<Cell> _attackArea;
        private Animator _animator;
        private SpriteRotator _spriteRotator;

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
            _actionPoints.PreTakenCurrentPoints -= cost;
            
            if(!_targetCell.IsEmpty)
                _targetCell.Content.health.PreTakeDamage(CalculateDamage());
        }

        public void Use()
        {
            _attackArea = PathFinder.Instance.GetCellsAreaForAOE(_unit.currentCell, radius, true, false);
            
            if (!PossibleToUseOnTarget(_targetCell.Content))
                return;
            
            if(_targetCell.Content == null || _targetCell.Content.health == null)
                return;
            
            if(!_actionPoints.IsActionPointsEnough(cost))
                return;
            
            _spriteRotator.TurnByTarget(_targetCell.Content);
            
            _animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, CharacterAnimatorParameters.ShieldIndex);
            _animator.SetTrigger(CharacterAnimatorParameters.Attack);

            _actionPoints.SpendPoints(cost);
            
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(RemoveFromCurrentlyActiveList), fullAnimationTime);
            Invoke(nameof(ApplyEffect), animationBeforeImpactTime);
            impactSound.Play();
        }

        public void ApplyEffect()
        {
            _targetCell.Content.health.TakeDamage(CalculateDamage());
        }

        private int CalculateDamage()
        {
            return _unit.currentCell.DistanceToCell(_targetCell) == radius
                ? criticalDamage
                : defaultDamage;
        }

        private void RemoveFromCurrentlyActiveList() => CurrentlyActiveObjects.Remove(this);

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.actionPoints;
            _grid = unit.Grid;
            unit.AbilitiesManager.AddAbility(this);
            _animator = unit.Animator;
            _animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, CharacterAnimatorParameters.ShieldIndex);
            _spriteRotator = unit.SpriteRotator;
        }

        public int GetCost() => cost;

        public bool PossibleToUseOnTarget(Unit target)
        {
            _attackArea = PathFinder.Instance.GetCellsAreaForAOE(_unit.currentCell, radius, true, false);
            return target != null && _attackArea.Contains(target.currentCell);
        }
        
        public void UseOnTarget(Unit target)
        {
            _targetCell = target.currentCell;
            Use();
        }

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
    }
}
