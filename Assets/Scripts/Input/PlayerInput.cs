using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IAbleToAct, IAbleToHaveCurrentAbility
{
    [SerializeField] private NewMovementAbility newMovementAbility;
    public bool InputIsPossible => true;

    public void Act()
    {
        newMovementAbility.CalculateUsingArea();
        var targetCells = new List<Cell> { CellsTaker.TakeCellByMouseRaycast() };
        var path = newMovementAbility.SelectCells(targetCells);
        newMovementAbility.VisualizePreUseOnCells(path);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            newMovementAbility.UseOnCells(path);
        }
    }

    public void ClearCurrentAbility()
    {

    }

    public IAbility GetCurrentAbility()
    {
        return null;
    }

    public void Init()
    {
        newMovementAbility.Init(GetComponentInParent<Unit>());
    }

    public void SetCurrentAbility(IAbility ability)
    {

    }
}
