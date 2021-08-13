using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : BaseInput
{
    private bool _inputIsPossible;

    public override void Act()
    {
        _inputIsPossible = true;
    }

    private void Update()
    {
        if (!_inputIsPossible)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.up));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.down));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.left));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>().FindNeigborhoodForCell(_unit._currentCell, Vector2Int.right));
        }

        _inputIsPossible = false;
    }
}
