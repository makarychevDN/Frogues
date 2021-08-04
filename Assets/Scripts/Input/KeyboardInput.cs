using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private bool _inputIsPossible;

    private void Update()
    {
        if (CurrentlyActiveObjects.SomethingIsActNow)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _unit._movable.Move(FindObjectOfType<MapBasedOnTilemap>()._unitsLayer[0,0]);
        }
    }
}
