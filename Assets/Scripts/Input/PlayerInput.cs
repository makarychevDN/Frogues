using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FroguesFramework
{
    public class PlayerInput : MonoBehaviour, IAbleToAct, IAbleToHaveCurrentAbility, IAbleToHaveNativeAttack
    {
        [SerializeField] private MovementAbility movementAbility;
        [SerializeField] private InspectAbility inspectAbility;
        [SerializeField] private AbleToUseAbility currentAbility;
        [SerializeField] private UnitTargetAbility nativeAttackAbility;
        [SerializeField] private HowerOnUnitWhileMovementMode howerOnUnitWhileMovementMode;

        [Header("Cursors")]
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D attackIsPossibleCursorTexture;
        [SerializeField] private Texture2D attackIsNotPossibleCursorTexture;
        [SerializeField] private Texture2D inspectAbilityCursorTexture;

        [SerializeField] private Texture2D moveCameraLeftTopCursorTexture;
        [SerializeField] private Texture2D moveCameraTopCursorTexture;
        [SerializeField] private Texture2D moveCameraRightTopCursorTexture;
        [SerializeField] private Texture2D moveCameraLeftCursorTexture;
        [SerializeField] private Texture2D moveCameraRightCursorTexture;
        [SerializeField] private Texture2D moveCameraLeftBottomCursorTexture;
        [SerializeField] private Texture2D moveCameraBottomCursorTexture;
        [SerializeField] private Texture2D moveCameraRightBottomCursorTexture;
        private Dictionary<Vector2Int, Texture2D> moveCameraCursorsByVectorsDictionary;

        private Unit _unit;
        private int _lastHashOfAbility;

        public bool InputIsPossible => _isPlayersTurn;
        private bool _isPlayersTurn;

        public void Act() 
        {
            _isPlayersTurn = true;
            PlayersTurnInput();
        }

        private void LateUpdate()
        {
            CameraMovementInput();
        }

        #region PlayersTurnInput
        private void PlayersTurnInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ClearCurrentAbility();
            }

            if (currentAbility == null)
                currentAbility = movementAbility;

            var temporaryCurrentAbility = currentAbility;

            if (!IsMouseOverUI)
            {
                if (currentAbility == movementAbility)
                {
                    var target = CellsTaker.TakeCellOrUnitByMouseRaycast();
                    inspectAbility.ShowMovementHighlighting = howerOnUnitWhileMovementMode == HowerOnUnitWhileMovementMode.activateInspectAbility;

                    if (howerOnUnitWhileMovementMode == HowerOnUnitWhileMovementMode.activateNativeAttack && nativeAttackAbility != null)
                    {
                        if (target is Unit && target != _unit)
                            temporaryCurrentAbility = nativeAttackAbility;
                    }

                    if (howerOnUnitWhileMovementMode == HowerOnUnitWhileMovementMode.activateInspectAbility)
                    {
                        if (target is Unit && target)
                            temporaryCurrentAbility = inspectAbility;
                    }
                }
            }

            UniversalAbilityInput(temporaryCurrentAbility);

            if (temporaryCurrentAbility == movementAbility)
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);

            if (temporaryCurrentAbility == inspectAbility)
                Cursor.SetCursor(inspectAbilityCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void UniversalAbilityInput(BaseAbility baseAbility)
        {
            var target = UniversalPrepareAbilityToUse(baseAbility);

            if (IsMouseOverUI)
                ResetTarget(ref target);

            if (_lastHashOfAbility != (baseAbility as IAbleToCalculateHashFunctionOfPrevisualisation).CalculateHashFunctionOfPrevisualisation() + IsMouseOverUI.ToInt())
                UniversalPrevisualization(baseAbility, target);
            _lastHashOfAbility = (baseAbility as IAbleToCalculateHashFunctionOfPrevisualisation).CalculateHashFunctionOfPrevisualisation() + IsMouseOverUI.ToInt();

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

                if (currentAbility == null)
                    return;

                if (currentAbility == movementAbility)
                    return;

                if(!currentAbility.IsResoursePointsEnough() || currentAbility.GetCurrentCharges() < 1)
                    ClearCurrentAbility();
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

                Unit target;

                List<string> layers = new List<string>();
                if (unitAbility.CheckItUsableOnDefaultUnit())
                    layers.Add("Unit");
                if (unitAbility.CheckItUsableOnBloodSurfaceUnit())
                    layers.Add("Blood Surface");

                target = CellsTaker.TakeUnitByLayersWithMouseRaycast(layers);

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
        #endregion

        private void CameraMovementInput()
        {
            SetMouseLockMode();
            if (EntryPoint.Instance.PauseIsActive)
                return;

            EntryPoint.Instance.CameraController.Zoom(Input.GetAxis("Mouse ScrollWheel"));

            if (Input.GetKey(KeyCode.Space))
            {
                EntryPoint.Instance.CameraController.ResetCamera();
            }

            if (Input.GetKey(KeyCode.Mouse2))
            {
                EntryPoint.Instance.CameraController.RotateCameraAroundYAxis(Input.GetAxis("Mouse X"));
                EntryPoint.Instance.CameraController.RotateCameraAroundXAxis(Input.GetAxis("Mouse Y"));
            }

            Vector2 movementInput = Vector2.zero;
            if (!Input.GetKey(KeyCode.Mouse2))
                movementInput = CheckMouseOnBordrers();
            Vector2 keyBoardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (keyBoardInput != Vector2.zero)
                movementInput = keyBoardInput;
            EntryPoint.Instance.CameraController.Move(movementInput);
        }

        private Vector2Int CheckMouseOnBordrers()
        {
            var mousePositionRelativeBorders = Input.mousePosition;
            int x = 0;
            int y = 0;

            if (mousePositionRelativeBorders.x <= 0) x = -1;
            else if (mousePositionRelativeBorders.x >= Screen.width - 1) x = 1;
            if (mousePositionRelativeBorders.y <= 0) y = -1;
            else if (mousePositionRelativeBorders.y >= Screen.height - 1)  y = 1;
            Vector2Int mousePositionInput = new Vector2Int(x, y);

            if (mousePositionInput == Vector2.zero)
                return Vector2Int.zero;

            Vector2 offset = new Vector2(mousePositionInput.x == 1 ? 39 : 0, mousePositionInput.y == -1 ? 39 : 0);
            Cursor.SetCursor(moveCameraCursorsByVectorsDictionary[mousePositionInput], offset, CursorMode.ForceSoftware);
            return mousePositionInput;
        }

        private void SetMouseLockMode()
        {
            Cursor.lockState = EntryPoint.Instance.PauseIsActive ? CursorLockMode.None : CursorLockMode.Confined;
        }

        private bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();
        
        public void ClearCurrentAbility()
        {
            if (currentAbility == movementAbility)
            {
                if (howerOnUnitWhileMovementMode == HowerOnUnitWhileMovementMode.activateNativeAttack)
                {
                    currentAbility = inspectAbility;
                }

                if (howerOnUnitWhileMovementMode == HowerOnUnitWhileMovementMode.activateInspectAbility)
                {
                    currentAbility = nativeAttackAbility;
                }
            }

            else
                currentAbility = null;
        }

        public BaseAbility GetCurrentAbility() => currentAbility;

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _unit.AbleToSkipTurn.OnSkipTurn.AddListener(() => _isPlayersTurn = false);
            movementAbility.Init(_unit);
            currentAbility = movementAbility;

            moveCameraCursorsByVectorsDictionary = new Dictionary<Vector2Int, Texture2D>
            {
                { new Vector2Int(-1, 1), moveCameraLeftTopCursorTexture },
                { new Vector2Int(0, 1), moveCameraTopCursorTexture },
                { new Vector2Int(1, 1), moveCameraRightTopCursorTexture },

                { new Vector2Int(-1, 0), moveCameraLeftCursorTexture },
                { new Vector2Int(1, 0), moveCameraRightCursorTexture },

                { new Vector2Int(-1, -1), moveCameraLeftBottomCursorTexture },
                { new Vector2Int(0, -1), moveCameraBottomCursorTexture },
                { new Vector2Int(1, -1), moveCameraRightBottomCursorTexture }
            };
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

            currentAbility = (AbleToUseAbility)ability;
        }

        public void SetCurrentNativeAttack(IAbleToBeNativeAttack ableToBeNativeAttack)
        {
            nativeAttackAbility = ableToBeNativeAttack as UnitTargetAbility;
        }

        public UnitTargetAbility GetCurrentNativeAttack() => nativeAttackAbility;

        public enum HowerOnUnitWhileMovementMode
        {
            activateInspectAbility = 10,
            activateNativeAttack = 20
        }
    }

}