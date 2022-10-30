using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class PlayerInput : BaseInput
    {
        [SerializeField] private VisualizeSelectedCell selectedCellVisualizer;
        [SerializeField] private CellByMousePosition cellByMousePosition;
        [SerializeField] private UnitsUIEnabler unitsUIEnabler;
        [SerializeField] private SpriteRotator spriteRotator;

        [Header("Movement Input")] [SerializeField]
        private HighlightValidForMovementCells movementCellsHighlighter;

        [SerializeField] private FindWayInValidCells findWayInValidCells;
        [SerializeField] private VisualizePath pathVisualizer;

        [Header("Abilities")] [SerializeField] private Weapon inspectAbility;
        [SerializeField] private Weapon nativeAbility;
        public Weapon currentAbility;

        [Header("Cursor Icons")] [SerializeField]
        private Sprite defaultCursor;

        [SerializeField] private Sprite attackCursor;
        [SerializeField] private Sprite attackIsNotPossibleCursor;
        [SerializeField] private Sprite inspectCursor;

        [SerializeField] private ActionPoints actionPoints;

        private List<Cell> _path = new();
        private float bottomUiPanelHeight = 120f;
        private ActionPointsIconsShaker _actionPointsIconsShaker;
        private PauseIsActiveContainer _pauseIsActiveContainer;

        private void Awake()
        {
            _pauseIsActiveContainer = FindObjectOfType<PauseIsActiveContainer>();
            _actionPointsIconsShaker = FindObjectOfType<ActionPointsIconsShaker>();
        }

        public bool InputIsPossible => UnitsQueue.Instance.IsUnitCurrent(unit)
                                       && !CurrentlyActiveObjects.SomethingIsActNow
                                       && !_pauseIsActiveContainer.Content;

        public override void Act(){}

        private void Update()
        {
            //Cursor.SetCursor(defaultCursor.texture, Vector2.zero, CursorMode.ForceSoftware);
            DisableAllVisualizationFromPlayerOnMap();
            //unitsUIEnabler.AllUnitsUISetActive(false);

            if (!InputIsPossible)
                return;

            if (_path.Count != 0)
            {
                unit.movable.Move(_path[0]);
                _path.RemoveAt(0);
                return;
            }

            MovementInput();
            
            /*selectedCellVisualizer.ApplyEffect();

            ResetPreCostContainers();

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (currentAbility != null)
                {
                    currentAbility = null;
                }
                else
                {
                    currentAbility = inspectAbility;
                }
            }

            if (currentAbility != null)
            {
                AbilityInput(currentAbility);
            }
            else
            {
                MovementInput();
            }

            unitsUIEnabler.AllUnitsUISetActive(true);*/
        }

        public void DisableAllVisualizationFromPlayerOnMap()
        {
            Map.Instance.allCells.ForEach(cell => cell.DisableAllCellVisualization());
            var cellsWithContent = Map.Instance.allCells.WithContentOnly();
            cellsWithContent.Where(cell => cell.Content.health != null).ToList()
                .ForEach(cell => cell.Content.health.ResetPreDamageValue());
            cellsWithContent.Where(cell => cell.Content.pushable != null).ToList()
                .ForEach(cell => cell.Content.pushable.ResetPrePushValue());
            cellsWithContent.Where(cell => cell.Content.actionPoints != null).ToList()
                .ForEach(cell => cell.Content.actionPoints.ResetPreCostValue());
        }

        private void MovementInput()
        {
            var movementArea = PathFinder.Instance.GetCellsAreaByActionPoints(unit.currentCell, actionPoints.CurrentActionPoints,
                unit.movable.DefaultMovementCost, false, false, true);
            movementArea.ForEach(cell => cell.EnableValidForMovementCellHighlight(movementArea));
            
            var grid = Map.Instance.tilemap.layoutGrid;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
            Cell targetCell;
            
            try
            {
                targetCell = Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y];
            }
            catch (IndexOutOfRangeException e)
            {
                return;
            }

            if(!movementArea.Contains(targetCell))
                return;
            
            targetCell.EnablePathDot(true);
            
            var path = PathFinder.Instance.FindWay(unit.currentCell, targetCell, false,
                false, true);

            if (path == null)
                return;
            
            path.Insert(0, unit.currentCell);

            path.GetLast().EnablePathDot(true);

            if (path.Count == 1)
                return;

            for (int i = 1; i < path.Count - 1; i++)
            {
                path[i].EnableTrail((path[i - 1].transform.position - path[i].transform.position).normalized.ToVector2());
                path[i].EnableTrail((path[i + 1].transform.position - path[i].transform.position).normalized.ToVector2());
            }

            path[0].EnableTrail((path[1].transform.position - path[0].transform.position).normalized.ToVector2());
            path[path.Count - 1].EnableTrail((path[path.Count - 2].transform.position - path[path.Count - 1].transform.position).normalized.ToVector2());

            actionPoints.PreTakenCurrentPoints -= path.Count - 1;
        }

        private void AbilityInput(Weapon ability)
        {
            spriteRotator.TurnByMousePosition();
            ability.HighlightCells();
            //preCostContainer.Content = ability.CurrentActionCost;

            Sprite currentCursor = ability.PossibleToUse ? attackCursor : attackIsNotPossibleCursor;
            if (ability == inspectAbility)
                currentCursor = inspectCursor;


            /*Cursor.SetCursor(
                currentCursor.texture,
                Vector2.zero,
                CursorMode.ForceSoftware);*/

            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
            {
                if (!ability.IsActionPointsEnough())
                    _actionPointsIconsShaker.Shake();

                ability.Use();
            }
        }

        public void SetCurrentAbility(Ability ability) => currentAbility = ability;

        public void SetCurrentAbilityNull() => currentAbility = null;
    }
}
