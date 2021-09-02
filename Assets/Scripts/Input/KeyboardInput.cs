using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : BaseInput
{
    private bool _inputIsPossible;
    public HighlightCells _cellsHighlighter;
    public VisualizePath _pathVisualizer;
    [SerializeField] private FindWayInValidCells _findWayIndValicCells;
    private List<Cell> _path = new List<Cell>();

    public override void Act()
    {
        _inputIsPossible = true;
    }

    private void Update()
    {
        if (!_inputIsPossible)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _path = _findWayIndValicCells.Take() == null ? new List<Cell>() : _findWayIndValicCells.Take();
        }

        if (_path.Count != 0)
        {
            _cellsHighlighter.TurnOffHighlight();
            _pathVisualizer.TurnOffVisualization();
            _inputIsPossible = false;
            
            _unit._movable.Move(_path[0]);
            _path.RemoveAt(0);
        }
        else
        {
            _pathVisualizer.ApplyEffect();
            _cellsHighlighter.ApplyEffect();
        }
    }
}
