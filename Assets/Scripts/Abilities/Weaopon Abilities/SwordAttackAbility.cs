using System.Collections.Generic;
using System.Linq;
using FroguesFramework;
using UnityEngine;

namespace FroguesFramework
{
    public class SwordAttackAbility : MonoBehaviour, IAbility, IAbleToDrawAbilityButton
    {
        [SerializeField] private int defaultDamage;
        [SerializeField] private int radius;
        [SerializeField] private int cost;
        [SerializeField] private float animationBeforeImpactTime;
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        [SerializeField] private AudioSource impactSound;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private Cell _targetCell;
        private List<Cell> _additionalCells;
        private List<Cell> _cellsToApplyEffect;
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
            _additionalCells = CellsTaker.TakeCellsAreaByRange(_targetCell, radius)
                .Where(cell => _attackArea.Contains(cell)).ToList();
            _additionalCells.ForEach(cell => cell.EnableSelectedCellHighlight(true));
            _actionPoints.PreSpendPoints(cost);
            _cellsToApplyEffect = _additionalCells;
            _cellsToApplyEffect.Add(_targetCell);

            foreach (var cell in _cellsToApplyEffect)
            {
                if(!cell.IsEmpty)
                    cell.Content.Health.PreTakeDamage(defaultDamage);
            }
        }

        public void Use()
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            
            if (_targetCell == null)
                return;

            if(!_actionPoints.IsActionPointsEnough(cost))
                return;
            
            _spriteRotator.TurnAroundByTarget(_targetCell.transform.position);
            
            _animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, CharacterAnimatorParameters.SwordIndex);
            _animator.SetTrigger(CharacterAnimatorParameters.Attack);

            _actionPoints.SpendPoints(cost);
            
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(RemoveFromCurrentlyActiveList), fullAnimationTime);
            Invoke(nameof(ApplyEffect), animationBeforeImpactTime);
            impactSound.Play();
        }

        public void ApplyEffect()
        {
            foreach (var cell in _cellsToApplyEffect)
            {
                if(!cell.IsEmpty)
                    cell.Content.Health.TakeDamage(defaultDamage);
            }
        }

        private void RemoveFromCurrentlyActiveList() => CurrentlyActiveObjects.Remove(this);

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.ActionPoints;
            //unit.AbilitiesManager.AddAbility(this);
            _animator = unit.Animator;
            _animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, CharacterAnimatorParameters.SwordIndex);
            _animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
            _spriteRotator = unit.SpriteRotator;
        }

        public int GetCost() => cost;

        public bool IsPartOfWeapon() => true;

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
    }
}
