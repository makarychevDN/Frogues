using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : BaseInput
{
    [SerializeField] private FindWayInValidCells findWayInValidCells;
    
    [SerializeField] private HighlightCells cellsHighlighter;
    [SerializeField] private VisualizePath pathVisualizer;
    [SerializeField] private VisualizeSelectedCell selectedCellVisualizer;
    
    private List<Cell> _path = new List<Cell>();
    private bool _inputIsPossible;

    public override void Act()
    {
        _inputIsPossible = true;
    }

    private void Update()
    {
        if (!_inputIsPossible)
            return;
        
        selectedCellVisualizer.ApplyEffect();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _path = findWayInValidCells.Take() == null ? new List<Cell>() : findWayInValidCells.Take();
        }

        if (_path.Count != 0)
        {
            cellsHighlighter.TurnOffHighlight();
            pathVisualizer.TurnOffVisualization();
            selectedCellVisualizer.TurnOffVizualisation();
            _inputIsPossible = false;
            
            unit.movable.Move(_path[0]);
            _path.RemoveAt(0);
        }
        else
        {
            pathVisualizer.ApplyEffect();
            cellsHighlighter.ApplyEffect();
        }
    }
}
