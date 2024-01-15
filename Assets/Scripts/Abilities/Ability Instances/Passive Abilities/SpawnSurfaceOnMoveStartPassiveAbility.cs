using FroguesFramework;
using UnityEngine;

public class SpawnSurfaceOnMoveStartPassiveAbility : PassiveAbility
{
    [SerializeField] private Unit surfacePrefab;

    public override void Init(Unit unit)
    {
        base.Init(unit);
        _owner.Movable.OnMovementStartFromCell.AddListener(SpawnSurfaceUnderOwner);
    }

    public override void UnInit()
    {
        base.UnInit();
        _owner.Movable.OnMovementStartFromCell.RemoveListener(SpawnSurfaceUnderOwner);
    }

    private void SpawnSurfaceUnderOwner(Cell cell)
    {
        if (cell != null)
            EntryPoint.Instance.SpawnUnit(surfacePrefab, cell);
    }
}
