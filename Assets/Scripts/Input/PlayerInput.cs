using FroguesFramework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IAbleToAct, IAbleToHaveCurrentAbility
{
    [SerializeField] private NewMovementAbility newMovementAbility;
    [SerializeField] private BaseAbility currentAbility;
    private Unit _unit;
    [SerializeField] private Unit _bonfire;

    public bool InputIsPossible => true;

    public void Act()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClearCurrentAbility();
        }

        if (currentAbility == null)
            currentAbility = newMovementAbility;

        if(currentAbility is IAbleToUseOnCells)
        {
            var currentCellsAbility = (AreaTargetAbility)currentAbility;

            currentCellsAbility.CalculateUsingArea();
            var targetCells = new List<Cell> { CellsTaker.TakeCellByMouseRaycast() };
            var path = newMovementAbility.SelectCells(targetCells);
            currentCellsAbility.VisualizePreUseOnCells(path);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentCellsAbility.UseOnCells(path);
            }
        }

        if (currentAbility is IAbleToUseOnUnit)
        {
            var currentUnitAbility = (UnitTargetAbility)currentAbility;

            currentUnitAbility.CalculateUsingArea();
            currentUnitAbility.VisualizePreUseOnUnit(_bonfire);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentUnitAbility.UseOnUnit(_bonfire);
            }
        }
    }

    public void ClearCurrentAbility() => currentAbility = null;

    public BaseAbility GetCurrentAbility() => currentAbility;

    public void Init()
    {
        _unit = GetComponentInParent<Unit>();
        newMovementAbility.Init(_unit);
        currentAbility = newMovementAbility;
    }

    public void SetCurrentAbility(BaseAbility ability) => currentAbility = ability;
}
