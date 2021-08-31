using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : BaseInput
{
    private bool _inputIsPossible;
    public HighlightCells _cellsHighlighter;

    public override void Act()
    {
        _inputIsPossible = true;
    }

    private void Update()
    {
        if (!_inputIsPossible)
            return;

        _cellsHighlighter.ApplyEffect();

        if (Input.GetKeyDown(KeyCode.W))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.up));
            _inputIsPossible = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.down));
            _inputIsPossible = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.left));
            _inputIsPossible = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.right));
            _inputIsPossible = false;
        }
    }
}
