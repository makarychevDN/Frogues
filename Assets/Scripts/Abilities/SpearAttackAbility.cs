
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
        private Cell _targetCell;
        private List<Cell> _attackArea;
        private Animator _animator;
        private SpriteRotator _spriteRotator;

        public void VisualizePreUse()
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            _attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_attackArea));
            _targetCell = CellsTaker.TakeCellByMouseRaycast();

            if (!_attackArea.Contains(_targetCell))
                return;
            
            _targetCell.EnableSelectedCellHighlight(true);
            _actionPoints.PreSpendPoints(cost);
            
            if(!_targetCell.IsEmpty)
                _targetCell.Content.Health.PreTakeDamage(CalculateDamage());
        }

        public void Use()
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            
            if (!PossibleToUseOnTarget(_targetCell.Content))
                return;
            
            if(_targetCell.Content == null || _targetCell.Content.Health == null)
                return;
            
            if(!_actionPoints.IsActionPointsEnough(cost))
                return;
            
            _spriteRotator.TurnAroundByTarget(_targetCell.transform.position);
            
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
            _targetCell.Content.Health.TakeDamage(CalculateDamage());
        }

        private int CalculateDamage()
        {
            return _unit.CurrentCell.DistanceToCell(_targetCell) == radius
                ? criticalDamage
                : defaultDamage;
        }

        private void RemoveFromCurrentlyActiveList() => CurrentlyActiveObjects.Remove(this);

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.ActionPoints;
            unit.AbilitiesManager.AddAbility(this);
            _animator = unit.Animator;
            _animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, CharacterAnimatorParameters.ShieldIndex);
            _spriteRotator = unit.SpriteRotator;
        }

        public int GetCost() => cost;

        public bool PossibleToUseOnTarget(Unit target)
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            return target != null && _attackArea.Contains(target.CurrentCell);
        }
        
        public void UseOnTarget(Unit target)
        {
            _targetCell = target.CurrentCell;
            Use();
        }

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
    }
}
