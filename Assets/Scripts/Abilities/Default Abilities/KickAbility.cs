using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class KickAbility : MonoBehaviour, IAbility, IAbleToUseOnTarget, IAbleToDrawAbilityButton, IAbleToDisablePreVisualization
    {
        [SerializeField] private int radius;
        [SerializeField] private int cost;
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        
        [Header("Visualization")]
        [SerializeField] private float animationBeforeImpactTime;
        [SerializeField] private float pushAnimationHeight;
        [SerializeField] private float pushAnimationSpeed;
        [SerializeField] private LineRenderer visualizationPreUseArrow;
        [SerializeField] private AnimationCurve animationCurve;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private Cell _targetCell;
        private List<Cell> _attackArea;
        private Animator _animator;
        private SpriteRotator _spriteRotator;
        
        public void VisualizePreUse()
        {
            visualizationPreUseArrow.gameObject.SetActive(false);
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            _attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_attackArea));
            _targetCell = CellsTaker.TakeCellByMouseRaycast();

            if (!_attackArea.Contains(_targetCell))
                return;
            
            _targetCell.EnableSelectedCellHighlight(true);
            _actionPoints.PreSpendPoints(cost);

            if(_targetCell.Content == null)
                return;
            
            visualizationPreUseArrow.gameObject.SetActive(true);

            float curveDelta = 1.0f / visualizationPreUseArrow.positionCount;
            for (int i = 0; i < visualizationPreUseArrow.positionCount; i++)
            {
                var position = PositionOnCurveCalculator.Calculate(_targetCell,
                    CellsTaker.JumpOverNeighborCell(_unit.CurrentCell, _targetCell), animationCurve,
                    curveDelta * (i + 1), pushAnimationHeight);

                position -= _unit.transform.position;
                
                visualizationPreUseArrow.SetPosition(i, position); 
            }
            
        }

        public void Use()
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            
            if (_targetCell == null || !PossibleToUseOnTarget(_targetCell.Content))
                return;
            
            if(_targetCell.Content == null || _targetCell.Content.Health == null)
                return;
            
            if(!_actionPoints.IsActionPointsEnough(cost))
                return;
            
            _spriteRotator.TurnAroundByTarget(_targetCell.transform.position);
            
            _animator.SetTrigger(CharacterAnimatorParameters.Kick);

            _actionPoints.SpendPoints(cost);
            
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(RemoveFromCurrentlyActiveList), fullAnimationTime);
            Invoke(nameof(ApplyEffect), animationBeforeImpactTime);
        }

        public void ApplyEffect()
        {
            _targetCell.Content.Movable.FreeCostMove(CellsTaker.JumpOverNeighborCell(_unit.CurrentCell, _targetCell),
                pushAnimationSpeed, pushAnimationHeight, true, false);
        }

        private void RemoveFromCurrentlyActiveList() => CurrentlyActiveObjects.Remove(this);

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.ActionPoints;
            unit.AbilitiesManager.AddAbility(this);
            _animator = unit.Animator;
            _spriteRotator = unit.SpriteRotator;
        }

        public int GetCost() => cost;

        public bool IsPartOfWeapon() => false;

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
        
        public void DisablePreVisualization()
        {
            visualizationPreUseArrow.gameObject.SetActive(false);
        }
    }
}