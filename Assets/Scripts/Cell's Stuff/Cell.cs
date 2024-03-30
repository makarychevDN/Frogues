using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    [ExecuteAlways]
    public class Cell : MonoBehaviour, IAbleToDisablePreVisualization
    {
        [field: SerializeField] public bool ChosenToMovement { get; set; }
        [field : SerializeField] public Vector2Int Coordinates { get; set; }
        [field : SerializeField] public Room ParentRoom { get; set; }

        [SerializeField] private Unit content;
        [SerializeField] private List<Unit> surfaces = new();

        [Header("Previsualization Setup")]
        [SerializeField] private CellHighlighter validForMovementTileHighlighter;
        [SerializeField] private CellHighlighter validForAbilityTileHighlighter;
        [SerializeField] private CellHighlighter selectedByAbilityTileHighlighter;
        [SerializeField] private TrailsEnabler trailsEnabler;
        [SerializeField] private SpriteRenderer pathDot;

        [Header("Links")]
        [SerializeField] private HexagonCellNeighbours hexagonCellNeighbours;

        public UnityEvent OnBecameFull = new();
        public UnityEvent<Unit> OnBecameFullByUnit = new();
        public UnityEvent OnBecameEmpty = new();
        public UnityEvent<Unit> OnBecameEmptyByUnit = new();
        public UnityEvent<Unit, Room> OnSomeoneSteppedInMyRoom = new();

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
                    OnSomeoneSteppedInMyRoom.Invoke(content, ParentRoom);
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

        public void ClampUnitToCell(Unit unit)
        {
            if (Application.isPlaying)
                return;

            content = unit;
            unit.transform.position = transform.position;
            unit.CurrentCell = this;
        }

        public bool IsEmpty => Content == null && !ChosenToMovement;
        
        public bool AbleToStepOnIt => (Content == null || Content.Small) && !ChosenToMovement;

        public HexagonCellNeighbours CellNeighbours => hexagonCellNeighbours;

        public bool CheckCellIsEmptyExtended(bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            if (!ignoreDefaultUnits && !EntryPoint.Instance.Map.CellsArray[Coordinates.x, Coordinates.y].IsEmpty)
            {
                if (ignoreSmallUnits && EntryPoint.Instance.Map.CellsArray[Coordinates.x, Coordinates.y].Content
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
            Coordinates = new Vector2Int(tilemap.WorldToCell(transform.position).x, tilemap.WorldToCell(transform.position).y);

            if (content == null)
                return;

            ClampUnitToCell(content);
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