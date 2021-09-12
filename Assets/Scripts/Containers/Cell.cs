using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject pathDot;
    [SerializeField] private GameObject onMouseHoverVisualization;

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
        pathDot.SetActive(isOn);
    }
    
    public void EnableOnMouseHoverVisualization(bool isOn)
    {
        onMouseHoverVisualization.SetActive(isOn);
    }

    public void DisableAllVisualization()
    {
        EnableSelectedCellHighlight(false);
        EnableValidateCellHighlight(false);
        EnablePathDot(false);
        EnableOnMouseHoverVisualization(false);
    }
}
