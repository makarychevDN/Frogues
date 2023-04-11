using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SpawnAndMoveProjectileInTheTargetAbility : MonoBehaviour, IAbility, IAbleToUseOnUnit, IAbleToDrawAbilityButton, IAbleToDisablePreVisualization
    {
        [SerializeField] private int radius;
        [SerializeField] private int cost;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        [SerializeField] private Unit projectilePrefab;
        [SerializeField] private bool needToRotateSpriteOnUse;
        
        [Header("Visualization")]
        [SerializeField] private float fullAnimationTime;
        [SerializeField] private float animationBeforeImpactTime;
        [SerializeField] private float projectileAnimationHeight;
        [SerializeField] private LineRenderer visualizationPreUseArrow;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private AudioSource spawnSound;
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
                var position = PositionOnCurveCalculator.Calculate(_unit.CurrentCell,
                    _targetCell, animationCurve, curveDelta * (i + 1), projectileAnimationHeight);

                position -= _unit.transform.position;
                
                visualizationPreUseArrow.SetPosition(i, position); 
            }
            
        }

        public void Use()
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            
            if (_targetCell == null || !PossibleToUseOnUnit(_targetCell.Content))
                return;
            
            if(_targetCell.Content == null || _targetCell.Content.Health == null)
                return;
            
            if(!_actionPoints.IsActionPointsEnough(cost))
                return;
            
            if(needToRotateSpriteOnUse)
                _spriteRotator.TurnAroundByTarget(_targetCell.transform.position);
            
            _animator.SetTrigger(CharacterAnimatorParameters.Attack);

            _actionPoints.SpendPoints(cost);
            
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(RemoveFromCurrentlyActiveList), fullAnimationTime);
            Invoke(nameof(ApplyEffect), animationBeforeImpactTime);
        }

        public void ApplyEffect()
        {
            if(spawnSound != null)
                spawnSound.Play();
            
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.Init();
            projectile.CurrentCell = _unit.CurrentCell;
            projectile.Movable.FreeCostMove(_targetCell, false);
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

        public bool PossibleToUseOnUnit(Unit target)
        {
            _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
            return target != null && _attackArea.Contains(target.CurrentCell);
        }
        
        public void UseOnUnit(Unit target)
        {
            _targetCell = target.CurrentCell;
            Use();
        }

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
        
        public void DisablePreVisualization()
        {
            visualizationPreUseArrow.gameObject.SetActive(false);
        }

        public void VisualizePreUseOnUnit(Unit target)
        {
            throw new System.NotImplementedException();
        }
    }
}
