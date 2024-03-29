using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace FroguesFramework
{
    [ExecuteInEditMode]
    public class Cell : MonoBehaviour, IAbleToDisablePreVisualization
    {
        public MapLayer mapLayer;
        [field : SerializeField] public Vector2Int coordinates { get; set; }

        public UnityEvent OnBecameFull = new();
        public UnityEvent OnBecameEmpty = new();

        public UnityEvent<Unit> OnBecameFullByUnit = new();
        public UnityEvent<Unit> OnBecameEmptyByUnit = new();

        [SerializeField] private Unit content;
        [SerializeField] private List<Unit> surfaces = new();
        [SerializeField] private CellHighlighter validForMovementTileHighlighter;
        [SerializeField] private CellHighlighter validForAbilityTileHighlighter;
        [SerializeField] private CellHighlighter selectedByAbilityTileHighlighter;
        [SerializeField] private TrailsEnabler trailsEnabler;
        [SerializeField] private SpriteRenderer pathDot;
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
                    EntryPoint.Instance.InvokeSomeoneMoved();
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
            if (!ignoreDefaultUnits && !EntryPoint.Instance.Map.CellsArray[coordinates.x, coordinates.y].IsEmpty)
            {
                if (ignoreSmallUnits && EntryPoint.Instance.Map.CellsArray[coordinates.x, coordinates.y].Content
                    .Small)
                    return true;

                return false;
            }

            return true;
        }

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

        private void Update()
        {
            if (Application.isPlaying) 
                return;

            if (!transform.hasChanged)
                return;

            Tilemap tilemap = GetComponentInParent<Tilemap>();
            if (tilemap == null) 
                return;

            transform.position = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
            coordinates = new Vector2Int(tilemap.WorldToCell(transform.position).x, tilemap.WorldToCell(transform.position).y);
        }

        private void OnDestroy()
        {
            transform.GetComponentInParent<Map>()?.RemoveCell(this);
            //RemoveMySelfFromEntryPoint();
        }

        public void DisablePreVisualization()
        {
            EnableSelectedByAbilityCellHighlight(false);
            EnableValidForAbilityCellHighlight(false);
            DisableTrails();
            EnablePathDot(false);
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