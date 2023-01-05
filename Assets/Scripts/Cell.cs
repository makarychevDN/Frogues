using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace FroguesFramework
{
    [ExecuteAlways]
    public class Cell : MonoBehaviour
    {
        public MapLayer mapLayer;
        [field : SerializeField] public Vector2Int coordinates { get; set; }

        public UnityEvent OnBecameFull;
        public UnityEvent OnBecameEmpty;
        
        [SerializeField] private Unit content;
        [SerializeField] private Transform hexagonModel;
        [SerializeField] private CellHighlighter validForMovementTileHighlighter;
        [SerializeField] private CellHighlighter validForAbilityTileHighlighter;
        [SerializeField] private GameObject selectedByAbilityTileHighlighter;
        [SerializeField] private TrailsEnabler trailsEnabler;
        [SerializeField] private SpriteRenderer pathDot;
        [SerializeField] private GameObject onMouseHoverVisualization;
        [SerializeField] private HexagonCellNeighbours hexagonCellNeighbours;
        [SerializeField] private Vector3 _hashedPosition;

        [ReadOnly] public bool chosenToMovement;

        public Unit Content
        {
            get => content;
            set
            {
                content = value;
                if (value != null)
                {
                    value.CurrentCell = this;
                    OnBecameFull.Invoke();
                }
                else
                {
                    OnBecameEmpty.Invoke();
                }
            }
        }

        public bool IsEmpty => Content == null && !chosenToMovement;

        public HexagonCellNeighbours CellNeighbours => hexagonCellNeighbours;

        public bool CheckColumnIsEmpty(bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            if (!ignoreDefaultUnits && !Map.Instance.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].IsEmpty)
            {
                if (ignoreSmallUnits && Map.Instance.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].Content
                    .Small)
                    return true;

                return false;
            }

            //if (!ignoreSurfaces/* && !Map.Instance.layers[MapLayer.Surface][coordinates.x, coordinates.y].IsEmpty*/)
                //return false;

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
        
        private void Update()
        {
            if (Application.isPlaying) 
                return;
            
            if (_hashedPosition == transform.localPosition)
                return;
            
            GetComponentInParent<Map>()?.SetCell(this);
            ClampPosition();
            coordinates = GetComponentInParent<Map>().GetGridPosition(this);
            _hashedPosition = transform.localPosition;
            hexagonModel.localRotation = Quaternion.Euler(90, Random.Range(0, 6) * 60, 0);
        }

        private void OnDestroy()
        {
            transform.GetComponentInParent<Map>()?.RemoveCell(this);
        }

        private void ClampPosition()
        {
            var zPos = transform.localPosition.z;
            zPos = (float)Math.Round(zPos / GridStep.Z) * GridStep.Z;
            
            var xPos = transform.localPosition.x - GridStep.X * 0.5f * (zPos / GridStep.Z % 2);
            xPos = (float)Math.Round(xPos / GridStep.X) * GridStep.X + GridStep.X * 0.5f * (zPos / GridStep.Z % 2);

            if (xPos <= 0 + GridStep.X * 0.5f * (zPos / GridStep.Z % 2) || zPos <= 0)
            {
                Debug.LogError("Cell need to be inside the grid (local x > 0 and local z > 0)");
                transform.GetComponentInParent<Map>()?.RemoveCell(this);
                return;
            }
            
            transform.localPosition = new Vector3(xPos, transform.localPosition.y, zPos);
        }
    }
}