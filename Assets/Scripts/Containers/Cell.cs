using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Cell : Container<Unit>
    {
        public MapLayer mapLayer;
        public Vector2Int coordinates;

        public UnityEvent OnBecameFull;
        public UnityEvent OnBecameEmpty;

        [SerializeField] private HighlightingBordersEnabler validForMovementTileHighlighter;
        [SerializeField] private GameObject validForAbilityTileHighlighter;
        [SerializeField] private GameObject selectedByAbilityTileHighlighter;
        [SerializeField] private TrailsEnabler trailsEnabler;
        [SerializeField] private SpriteRenderer pathDot;
        [SerializeField] private GameObject onMouseHoverVisualization;

        [ReadOnly] public bool chosenToMovement;

        public override Unit Content
        {
            get => base.Content;
            set
            {
                base.Content = value;
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

        public override bool IsEmpty => Content == null && !chosenToMovement;

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
            EnableValidForAbilityCellHighlight(false);
            selectedByAbilityTileHighlighter.gameObject.SetActive(isOn);
        }

        public void EnableValidForAbilityCellHighlight(bool isOn) => validForAbilityTileHighlighter.SetActive(isOn);

        public void EnableValidForMovementCellHighlight(bool isOn) => validForMovementTileHighlighter.SetActiveBorders(isOn);
        
        public void EnableValidForMovementCellHighlight(List<Cell> cells) => validForMovementTileHighlighter.EnableBorders(cells);

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