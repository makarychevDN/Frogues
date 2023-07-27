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
    public class Cell : MonoBehaviour, IAbleToDisablePreVisualization
    {
        public MapLayer mapLayer;
        [field : SerializeField] public Vector2Int coordinates { get; set; }

        public UnityEvent OnBecameFull = new();
        public UnityEvent OnBecameEmpty = new();

        public UnityEvent<Unit> OnBecameFullByUnit = new();
        public UnityEvent<Unit> OnBecameEmptyByUnit = new();

        [SerializeField] private Unit content;
        [SerializeField] private List<Unit> surfaces;
        [SerializeField] private Transform hexagonModel;
        [SerializeField] private CellHighlighter validForMovementTileHighlighter;
        [SerializeField] private CellHighlighter validForAbilityTileHighlighter;
        [SerializeField] private CellHighlighter selectedByAbilityTileHighlighter;
        //[SerializeField] private GameObject selectedByAbilityTileHighlighter;
        [SerializeField] private TrailsEnabler trailsEnabler;
        [SerializeField] private SpriteRenderer pathDot;
        [SerializeField] private GameObject onMouseHoverVisualization;
        [SerializeField] private HexagonCellNeighbours hexagonCellNeighbours;
        [SerializeField] private Vector3 _hashedPosition;

        [ReadOnly] public bool chosenToMovement;

        public List<Unit> Surfaces => surfaces;

        public Unit Content
        {
            get => content;
            set
            {
                if (value != null)
                {
                    content = value;
                    value.CurrentCell = this;
                    OnBecameFull.Invoke();
                    OnBecameFullByUnit.Invoke(content);
                }
                else
                {
                    OnBecameEmpty.Invoke();
                    OnBecameEmptyByUnit.Invoke(content);
                    content = value;
                }
            }
        }

        public bool IsEmpty => Content == null && !chosenToMovement;
        
        public bool AbleToStepOnIt => (Content == null || Content.Small) && !chosenToMovement;

        public HexagonCellNeighbours CellNeighbours => hexagonCellNeighbours;

        public bool CheckColumnIsEmpty(bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            if (!ignoreDefaultUnits && !EntryPoint.Instance.Map.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].IsEmpty)
            {
                if (ignoreSmallUnits && EntryPoint.Instance.Map.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].Content
                    .Small)
                    return true;

                return false;
            }

            //if (!ignoreSurfaces/* && !Map.Instance.layers[MapLayer.Surface][coordinates.x, coordinates.y].IsEmpty*/)
                //return false;

            return true;
        }

        //public bool CheckColumnIsEmpty() =>
        //EntryPoint.Instance.Map.GetCellsColumnIgnoreSurfaces(coordinates).All(cell => cell.AbleToStepOnIt);

        /*public void EnableSelectedCellHighlight(bool isOn)
        {
            EnableValidForAbilityCellHighlightOnly(false);
            selectedByAbilityTileHighlighter.gameObject.SetActive(isOn);
        }*/

        public void EnableSelectedByAbilityCellHighlight(List<Cell> cells) 
        { 
            selectedByAbilityTileHighlighter.EnableBordersAndHighlight(cells);
            cells.ForEach(cell => cell.EnableValidForAbilityCellHighlight(false));
        }
        public void EnableSelectedByAbilityCellHighlight(bool isOn) => selectedByAbilityTileHighlighter.SetActive(isOn);
        public void EnableValidForAbilityCellHighlight(List<Cell> cells) => validForAbilityTileHighlighter.EnableBordersAndHighlight(cells);
        public void EnableValidForAbilityCellHighlight(bool isOn) => validForAbilityTileHighlighter.SetActive(isOn);
        
        public void EnableValidForAbilityCellHighlightOnly(bool isOn) => validForAbilityTileHighlighter.SetActiveHighlight(isOn);

        public void EnableValidForMovementCellHighlight(bool isOn) => validForMovementTileHighlighter.SetActive(isOn);
        
        public void EnableValidForMovementCellHighlight(List<Cell> cells) => validForMovementTileHighlighter.EnableBordersAndHighlight(cells);

        public void EnablePathDot(bool isOn) => pathDot.enabled = isOn;

        public void EnableTrail(Vector2 direction) => trailsEnabler.EnableTrail(direction);

        public void EnableTrail(Cell targetCell)
        {
            EnableTrail((targetCell.transform.position - transform.position).normalized.ToVector2ZasY());
        }

        public void DisableTrails() => trailsEnabler.DisableTrails();

        public void EnableOnMouseHoverVisualization(bool isOn) => onMouseHoverVisualization.SetActive(isOn);

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
            hexagonModel.localRotation = Quaternion.Euler(-90, Random.Range(0, 6) * 60, 0);

            if (Content != null)
            {
                Content.transform.position = transform.position;
            }

            surfaces.ForEach(surface => surface.transform.position = transform.position);
        }

        private void OnDestroy()
        {
            transform.GetComponentInParent<Map>()?.RemoveCell(this);
            //RemoveMySelfFromEntryPoint();
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

        public void DisablePreVisualization()
        {
            EnableSelectedByAbilityCellHighlight(false);
            EnableValidForAbilityCellHighlight(false);
            DisableTrails();
            EnablePathDot(false);
            EnableOnMouseHoverVisualization(false);
            EnableValidForMovementCellHighlight(false);
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        private void Start()
        {
            if (Application.isPlaying)
                AddMySelfToEntryPoint();
        }
    }
}