using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndMoveUnitAbility : MonoBehaviour, IAbility
{
    [SerializeField] private int radius;
    [SerializeField] private int cost;
    [SerializeField] private AbilityDataForButton abilityDataForButton;
    [SerializeField] private Unit unitToSpawn;
    [SerializeField] private bool needToRotateSpriteOnUse;

    [Header("Visualization")]
    [SerializeField] private float fullAnimationTime;
    [SerializeField] private float animationBeforeImpactTime;
    [SerializeField] private float projectileAnimationHeight;
    [SerializeField] private AudioSource spawnSound;
    private Unit _unit;
    private ActionPoints _actionPoints;
    private Cell _targetCell;
    private List<Cell> _attackArea;
    private Animator _animator;
    private SpriteRotator _spriteRotator;

    public void VisualizePreUse()
    {

    }

    public void Use()
    {
        _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
        _targetCell = _attackArea.EmptyCellsOnly().GetRandomElement();

        if (_targetCell == null || !PossibleToUse())
            return;

        if (!_actionPoints.IsActionPointsEnough(cost))
            return;

        if (needToRotateSpriteOnUse)
            _spriteRotator.TurnAroundByTarget(_targetCell.transform.position);

        _animator.SetTrigger(CharacterAnimatorParameters.Attack);
        _actionPoints.SpendPoints(cost);

        if (_targetCell == null)
            return;

        CurrentlyActiveObjects.Add(this);
        Invoke(nameof(RemoveFromCurrentlyActiveList), fullAnimationTime);
        Invoke(nameof(ApplyEffect), animationBeforeImpactTime);
    }

    public void ApplyEffect()
    {
        if (spawnSound != null)
            spawnSound.Play();

        var spawnedUnit = Instantiate(unitToSpawn, transform.position, Quaternion.identity);
        spawnedUnit.Init();
        spawnedUnit.CurrentCell = _unit.CurrentCell;
        spawnedUnit.Movable.FreeCostMove(_targetCell, 10, 1, false);
        EntryPoint.Instance.UnitsQueue.AddObjectInQueueAfterTarget(_unit, spawnedUnit);
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

    public bool PossibleToUse()
    {
        _attackArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);
        return _attackArea.EmptyCellsOnly().Count > 0;
    }
}