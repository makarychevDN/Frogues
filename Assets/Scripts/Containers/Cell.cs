using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Cell : Container<Unit>
{
    public MapLayer mapLayer;
    public Vector2Int coordinates;

    public UnityEvent OnBecameFull;
    public UnityEvent OnBecameEmpty;
    
    [SerializeField] private GameObject validateCellTileHighlighter;
    [SerializeField] private GameObject selectedCellTileHighlighter;
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
            if(value != null)
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

    public Cell GetNeighbor(Vector2Int direction)
    {
        return Map.Instance.GetLayerByCell(this)[coordinates.x + direction.x, coordinates.y + direction.y];
    }

    public bool CheckColumnIsEmpty(bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces) 
    {
        if (!ignoreDefaultUnits && !Map.Instance.layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y].IsEmpty) return false;
        if (!ignoreSmallUnits && !Map.Instance.layers[MapLayer.SmallUnit][coordinates.x, coordinates.y].IsEmpty) return false;
        if (!ignoreSurfaces && !Map.Instance.layers[MapLayer.Surface][coordinates.x, coordinates.y].IsEmpty) return false;
        return true;
    }

    public bool CheckColumnIsEmpty() => Map.Instance.GetCellsColumnIgnoreSurfaces(coordinates).All(cell => cell.IsEmpty);

    public void EnableSelectedCellHighlight(bool isOn)
    {
        EnableValidateCellHighlight(false);
        selectedCellTileHighlighter.gameObject.SetActive(isOn);
    }

    public void EnableValidateCellHighlight(bool isOn)
    {
        validateCellTileHighlighter.SetActive(isOn);
    }

    public void EnablePathDot(bool isOn)
    {
        pathDot.enabled = isOn;
    }

    public void EnableTrail(Vector2Int direction)
    {
        trailsEnabler.EnableTrail(direction);
    }

    public void DisableTrails()
    {
        trailsEnabler.DisableTrails();
    }

    public void EnableOnMouseHoverVisualization(bool isOn)
    {
        onMouseHoverVisualization.SetActive(isOn);
    }

    public void DisableAllCellVisualization()
    {
        EnableSelectedCellHighlight(false);
        EnableValidateCellHighlight(false);
        DisableTrails();
        EnablePathDot(false);
        EnableOnMouseHoverVisualization(false);
    }
}
