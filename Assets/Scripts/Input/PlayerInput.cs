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
        private int _lastHashOfAbility;

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

                if (target is Unit && target != _unit)
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

            if (temporaryCurrentAbility is IAbleToUseInDirectionOfCursorPosition)
            {
                AbleToUseInDirectionAbilityInput((DirectionOfCursorTargetAbility)temporaryCurrentAbility);
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

            unitAbility.PrepareToUsing(targetUnit);

            if(_lastHashOfAbility != unitAbility.CalculateHashFunctionOfPrevisualisation())
            {
                EntryPoint.Instance.DisableAllPrevisualization();
                unitAbility.VisualizePreUseOnUnit(targetUnit);
            }

            _lastHashOfAbility = unitAbility.CalculateHashFunctionOfPrevisualisation();

            if (!unitAbility.PossibleToUseOnUnit(targetUnit))
                Cursor.SetCursor(attackIsNotPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (IsMouseOverUI)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                EntryPoint.Instance.DisableAllPrevisualization();
                unitAbility.UseOnUnit(targetUnit);
            }
        }

        private void IAbleToUseOnCellsAbilityInput(AreaTargetAbility cellsAbility)
        {
            var targetCells = new List<Cell> { CellsTaker.TakeCellByMouseRaycast() };
            cellsAbility.PrepareToUsing(targetCells);
            var selectedCells = cellsAbility.SelectCells(targetCells);

            Cursor.SetCursor(attackIsPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (IsMouseOverUI)
            {
                selectedCells = null;
            }

            if (_lastHashOfAbility != cellsAbility.CalculateHashFunctionOfPrevisualisation())
            {
                EntryPoint.Instance.DisableAllPrevisualization();
                cellsAbility.VisualizePreUseOnCells(selectedCells);
            }

            _lastHashOfAbility = cellsAbility.CalculateHashFunctionOfPrevisualisation();

            if (!cellsAbility.PossibleToUseOnCells(selectedCells))
                Cursor.SetCursor(attackIsNotPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);


            if (IsMouseOverUI)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                EntryPoint.Instance.DisableAllPrevisualization();
                cellsAbility.UseOnCells(selectedCells);
            }
        }

        private void AbleToUseInDirectionAbilityInput(DirectionOfCursorTargetAbility directedAbility)
        {
            Vector3 cursorPosition = Input.mousePosition;

            Cursor.SetCursor(attackIsPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            directedAbility.PrepareToUsing(cursorPosition);

            if(_lastHashOfAbility != directedAbility.CalculateHashFunctionOfPrevisualisation())
            {
                EntryPoint.Instance.DisableAllPrevisualization();
                directedAbility.VisualizePreUseInDirection(cursorPosition);
            }

            _lastHashOfAbility = directedAbility.CalculateHashFunctionOfPrevisualisation();

            if (!directedAbility.PossibleToUseInDirection(cursorPosition))
                Cursor.SetCursor(attackIsNotPossibleCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (IsMouseOverUI)
            {
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                EntryPoint.Instance.DisableAllPrevisualization();
                directedAbility.UseInDirection(cursorPosition);
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
                _lastHashOfAbility = 0;
                EntryPoint.Instance.DisableAllPrevisualization();
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