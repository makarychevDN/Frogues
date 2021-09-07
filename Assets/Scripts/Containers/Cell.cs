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

    [SerializeField] private SpriteRenderer tileHighlighter;
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

    public Color HighlightColor
    {
        set
        {
            tileHighlighter.color = new Color(value.r, value.g, value.b, 16);
        }
    }

    public void ResetColor()
    {
        tileHighlighter.color = new Color(255, 255, 255, 16);
    }

    public void EnableHighlight(bool isOn)
    {
        tileHighlighter.gameObject.SetActive(isOn);
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
