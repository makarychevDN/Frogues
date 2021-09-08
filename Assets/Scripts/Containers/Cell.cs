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
    
    [SerializeField] private GameObject defaultTileHighlighter;
    [SerializeField] private SpriteRenderer customTileHighlighter;
    [SerializeField] private byte customTileHighlighterAlpha; 
    [SerializeField] private GameObject pathDot;
    [SerializeField] private GameObject selectedVisualization;

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
    
    public void EnableCustomHighlight(bool isOn, Color color)
    {
        customTileHighlighter.color = new Color(color.r, color.g, color.b, 32);
        EnableCustomHighlight(isOn);
    }
    
    public void EnableCustomHighlight(bool isOn)
    {
        EnableDefaultHighlight(false);
        customTileHighlighter.gameObject.SetActive(isOn);
    }

    public void EnableDefaultHighlight(bool isOn)
    {
        defaultTileHighlighter.SetActive(isOn);
    }

    public void EnablePathDot(bool isOn)
    {
        pathDot.SetActive(isOn);
    }
    
    public void EnableSelectedVisualization(bool isOn)
    {
        selectedVisualization.SetActive(isOn);
    }
}
