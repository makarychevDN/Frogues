using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

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
            if (currentAbility == movementAbility && nativeAttackAbility != null)
            {
                var target = CellsTaker.TakeCellOrUnitByMouseRaycast();

                if (target is Unit && target != _unit)
                    temporaryCurrentAbility = nativeAttackAbility;
            }

            /*if (temporaryCurrentAbility is IAbleToUseOnCells)
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
            }*/

            UniversalAbilityInput(temporaryCurrentAbility);

            if (temporaryCurrentAbility == movementAbility)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (temporaryCurrentAbility == inspectAbility)
                Cursor.SetCursor(inspectAbilityCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void UniversalAbilityInput(BaseAbility baseAbility)
        {
            var target = UniversalPrepareAbilityToUse(baseAbility);

            if (baseAbility is IAbleToUseInDirectionOfCursorPosition)
                print(target == null);

            if (IsMouseOverUI)
                ResetTarget(ref target);

            if (baseAbility is IAbleToUseInDirectionOfCursorPosition)
                print(target == null);

            if (_lastHashOfAbility != (baseAbility as IAbleToCalculateHashFunctionOfPrevisualisation).CalculateHashFunctionOfPrevisualisation())
                UniversalPrevisualization(baseAbility, target);
            _lastHashOfAbility = (baseAbility as IAbleToCalculateHashFunctionOfPrevisualisation).CalculateHashFunctionOfPrevisualisation();

            Cursor.SetCursor(
                UniversalIsPossibleToUse(baseAbility, target) ? attackIsPossibleCursorTexture : attackIsNotPossibleCursorTexture, 
                Vector2.zero, 
                CursorMode.ForceSoftware);

            if (IsMouseOverUI)
            {
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                UniversalUseAbility(baseAbility, target);
            }
        }

        private System.Object UniversalPrepareAbilityToUse(BaseAbility baseAbility)
        {
            if (baseAbility is IAbleToUseOnCells)
            {
                IAbleToUseOnCells cellsAbility = (IAbleToUseOnCells)baseAbility;
                var targetCells = new List<Cell> { CellsTaker.TakeCellByMouseRaycast() };
                cellsAbility.PrepareToUsing(targetCells);
                return cellsAbility.SelectCells(targetCells);
            }

            if (baseAbility is IAbleToUseOnUnit)
            {
                IAbleToUseOnUnit unitAbility = (IAbleToUseOnUnit)baseAbility;
                var target = CellsTaker.TakeUnitByMouseRaycast();
                unitAbility.PrepareToUsing(target);
                return target;
            }

            if (baseAbility is IAbleToUseInDirectionOfCursorPosition)
            {
                IAbleToUseInDirectionOfCursorPosition cursorAbility = (IAbleToUseInDirectionOfCursorPosition)baseAbility;
                cursorAbility.PrepareToUsing(Input.mousePosition);
                return Input.mousePosition;
            }

            return null;
        }

        private void ResetTarget(ref System.Object target)
        {
            if (target is not Vector3)
            {
                target = null;
            }
        }

        private void UniversalPrevisualization(BaseAbility baseAbility, System.Object target)
        {
            EntryPoint.Instance.DisableAllPrevisualization();

            if (baseAbility is IAbleToUseOnCells)
            {
                ((IAbleToUseOnCells)baseAbility).VisualizePreUseOnCells((List<Cell>)target);
                return;
            }

            if (baseAbility is IAbleToUseOnUnit)
            {
                ((IAbleToUseOnUnit)baseAbility).VisualizePreUseOnUnit((Unit)target);
                return;
            }

            if (baseAbility is IAbleToUseInDirectionOfCursorPosition)
            {
                ((IAbleToUseInDirectionOfCursorPosition)baseAbility).VisualizePreUseInDirection((Vector3)target);
                return;
            }
        }

        private bool UniversalIsPossibleToUse(BaseAbility baseAbility, System.Object target)
        {
            if (baseAbility is IAbleToUseOnCells)
            {
                return ((IAbleToUseOnCells)baseAbility).PossibleToUseOnCells((List<Cell>)target);
            }

            if (baseAbility is IAbleToUseOnUnit)
            {
                return ((IAbleToUseOnUnit)baseAbility).PossibleToUseOnUnit((Unit)target);
            }

            if (baseAbility is IAbleToUseInDirectionOfCursorPosition)
            {
                return ((IAbleToUseInDirectionOfCursorPosition)baseAbility).PossibleToUseInDirection((Vector3)target);
            }

            return false;
        }

        private void UniversalUseAbility(BaseAbility baseAbility, System.Object target)
        {
            EntryPoint.Instance.DisableAllPrevisualization();

            if (baseAbility is IAbleToUseOnCells)
            {
                _lastHashOfAbility = 0;
                ((IAbleToUseOnCells)baseAbility).UseOnCells((List<Cell>)target);
                return;
            }

            if (baseAbility is IAbleToUseOnUnit)
            {
                _lastHashOfAbility = 0;
                ((IAbleToUseOnUnit)baseAbility).UseOnUnit((Unit)target);
                return;
            }

            if (baseAbility is IAbleToUseInDirectionOfCursorPosition)
            {
                _lastHashOfAbility = 0;
                ((IAbleToUseInDirectionOfCursorPosition)baseAbility).UseInDirection((Vector3)target);
                return;
            }
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
                _lastHashOfAbility = 0;
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
                _lastHashOfAbility = 0;
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
                _lastHashOfAbility = 0;
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