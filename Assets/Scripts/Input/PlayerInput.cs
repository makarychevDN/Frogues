using FroguesFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FroguesFramework
{
    public class PlayerInput : MonoBehaviour, IAbleToAct, IAbleToHaveCurrentAbility
    {
        [SerializeField] private MovementAbility movementAbility;
        [SerializeField] private BaseAbility currentAbility;
        [SerializeField] private Texture2D defaultCursorTexture;
        private Unit _unit;

        public bool InputIsPossible => true;

        public void Act()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ClearCurrentAbility();
            }

            if (currentAbility == null)
                currentAbility = movementAbility;

            if (currentAbility is IAbleToUseOnCells)
            {
                var currentCellsAbility = (AreaTargetAbility)currentAbility;

                currentCellsAbility.CalculateUsingArea();
                var targetCells = new List<Cell> { CellsTaker.TakeCellByMouseRaycast() };
                var selectedCells = currentCellsAbility.SelectCells(targetCells);

                if (IsMouseOverUI)
                    selectedCells = null;

                currentCellsAbility.VisualizePreUseOnCells(selectedCells);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    currentCellsAbility.UseOnCells(selectedCells);
                }
            }

            if (currentAbility is IAbleToUseOnUnit)
            {
                var currentUnitAbility = (UnitTargetAbility)currentAbility;
                var targetUnit = CellsTaker.TakeUnitByMouseRaycast();

                if (IsMouseOverUI)
                    targetUnit = null;

                currentUnitAbility.CalculateUsingArea();
                currentUnitAbility.VisualizePreUseOnUnit(targetUnit);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    currentUnitAbility.UseOnUnit(targetUnit);
                }
            }

            if (currentAbility == movementAbility)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();
        

        public void ClearCurrentAbility() => currentAbility = null;

        public BaseAbility GetCurrentAbility() => currentAbility;

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            movementAbility.Init(_unit);
            currentAbility = movementAbility;
        }

        //public void SetCurrentAbility(BaseAbility ability) => currentAbility = ability;
        public void SetCurrentAbility(BaseAbility ability)
        {
            if (ability is IAbleToUseWithNoTarget)
            {
                (ability as IAbleToUseWithNoTarget).Use();
                return;
            }

            currentAbility = ability;
        }
    }

}