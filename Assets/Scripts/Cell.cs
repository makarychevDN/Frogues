using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Cell : MonoBehaviour
    {
        public MapLayer mapLayer;
        public Vector2Int coordinates;

        public UnityEvent OnBecameFull;
        public UnityEvent OnBecameEmpty;

        [SerializeField] private Unit content;
        [SerializeField] private CellHighlighter validForMovementTileHighlighter;
        [SerializeField] private CellHighlighter validForAbilityTileHighlighter;
        [SerializeField] private GameObject selectedByAbilityTileHighlighter;
        [SerializeField] private TrailsEnabler trailsEnabler;
        [SerializeField] private SpriteRenderer pathDot;
        [SerializeField] private GameObject onMouseHoverVisualization;

        [ReadOnly] public bool chosenToMovement;

        public Unit Content
        {
            get => content;
            set
            {
                content = value;
                if (value != null)
                {
                    value.currentCell = this;
                    OnBecameFull.Invoke();
                }
                else
                {
                    OnBecameEmpty.Invoke();
                }
            }
        }

        public bool IsEmpty => Content == null && !chosenToMovement;

        public bool CheckColumnIsEmpty(bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            if (!ignoreDefaultUnits && !Map.Instance.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].IsEmpty)
            {
                if (ignoreSmallUnits && Map.Instance.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].Content
                    .small)
                    return true;

                return false;
            }

            if (!ignoreSurfaces && !Map.Instance.layers[MapLayer.Surface][coordinates.x, coordinates.y].IsEmpty)
                return false;

            return true;
        }

        public bool CheckColumnIsEmpty() =>
            Map.Instance.GetCellsColumnIgnoreSurfaces(coordinates).All(cell => cell.IsEmpty);

        public void EnableSelectedCellHighlight(bool isOn)
        {
            EnableValidForAbilityCellHighlightOnly(false);
            selectedByAbilityTileHighlighter.gameObject.SetActive(isOn);
        }

        public void EnableValidForAbilityCellHighlight(List<Cell> cells) => validForAbilityTileHighlighter.EnableBordersAndHighlight(cells);
        public void EnableValidForAbilityCellHighlight(bool isOn) => validForAbilityTileHighlighter.SetActive(isOn);
        
        public void EnableValidForAbilityCellHighlightOnly(bool isOn) => validForAbilityTileHighlighter.SetActiveHighlight(isOn);

        public void EnableValidForMovementCellHighlight(bool isOn) => validForMovementTileHighlighter.SetActive(isOn);
        
        public void EnableValidForMovementCellHighlight(List<Cell> cells) => validForMovementTileHighlighter.EnableBordersAndHighlight(cells);

        public void EnablePathDot(bool isOn) => pathDot.enabled = isOn;

        public void EnableTrail(Vector2 direction) => trailsEnabler.EnableTrail(direction);

        public void DisableTrails() => trailsEnabler.DisableTrails();

        public void EnableOnMouseHoverVisualization(bool isOn) => onMouseHoverVisualization.SetActive(isOn);

        public void DisableAllCellVisualization()
        {
            EnableSelectedCellHighlight(false);
            EnableValidForAbilityCellHighlight(false);
            DisableTrails();
            EnablePathDot(false);
            EnableOnMouseHoverVisualization(false);
            EnableValidForMovementCellHighlight(false);
        }
    }
}