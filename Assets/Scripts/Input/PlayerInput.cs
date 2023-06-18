using FroguesFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FroguesFramework
{
    public class PlayerInput : MonoBehaviour, IAbleToAct, IAbleToHaveCurrentAbility, IAbleToHaveNativeAttack
    {
        [SerializeField] private MovementAbility movementAbility;
        [SerializeField] private InspectAbility inspectAbility;
        [SerializeField] private BaseAbility currentAbility;
        [SerializeField] private UnitTargetAbility nativeAttackAbility;

        [Header("Cursors")]
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D attackIsPossibleCursorTexture;
        [SerializeField] private Texture2D attackIsNotPossibleCursorTexture;
        [SerializeField] private Texture2D inspectAbilityCursorTexture;
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

            var temporaryCurrentAbility = currentAbility;
            if(currentAbility == movementAbility && nativeAttackAbility != null)
            {
                var target = CellsTaker.TakeCellOrUnitByMouseRaycast();

                if (target is Unit)
                    temporaryCurrentAbility = nativeAttackAbility;
            }

            if (temporaryCurrentAbility is IAbleToUseOnCells)
            {
                IAbleToUseOnCellsAbilityInput((AreaTargetAbility)temporaryCurrentAbility);
            }

            if (temporaryCurrentAbility is IAbleToUseOnUnit)
            {
                AbleToUseOnUnitAbilityInput((UnitTargetAbility)temporaryCurrentAbility);
            }

            if (temporaryCurrentAbility == movementAbility)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (temporaryCurrentAbility == inspectAbility)
                Cursor.SetCursor(inspectAbilityCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void AbleToUseOnUnitAbilityInput(UnitTargetAbility unitAbility)
        {
            var targetUnit = CellsTaker.TakeUnitByMouseRaycast();

            Cursor.SetCursor(attackIsPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (IsMouseOverUI)
            {
                targetUnit = null;
            }

            unitAbility.CalculateUsingArea();
            unitAbility.VisualizePreUseOnUnit(targetUnit);

            if (!unitAbility.PossibleToUseOnUnit(targetUnit))
                Cursor.SetCursor(attackIsNotPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (IsMouseOverUI)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                unitAbility.UseOnUnit(targetUnit);
            }
        }

        private void IAbleToUseOnCellsAbilityInput(AreaTargetAbility cellsAbility)
        {
            cellsAbility.CalculateUsingArea();
            var targetCells = new List<Cell> { CellsTaker.TakeCellByMouseRaycast() };
            var selectedCells = cellsAbility.SelectCells(targetCells);

            Cursor.SetCursor(attackIsPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (IsMouseOverUI)
            {
                selectedCells = null;
            }

            cellsAbility.VisualizePreUseOnCells(selectedCells);

            if (!cellsAbility.PossibleToUseOnCells(selectedCells))
                Cursor.SetCursor(attackIsNotPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);


            if (IsMouseOverUI)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                cellsAbility.UseOnCells(selectedCells);
            }
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

        public void SetCurrentNativeAttack(IAbleToBeNativeAttack ableToBeNativeAttack)
        {
            nativeAttackAbility = ableToBeNativeAttack as UnitTargetAbility;
        }
    }

}