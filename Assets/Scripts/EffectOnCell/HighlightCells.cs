using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : MonoBehaviour
{
    [SerializeField] private BaseCellsTaker _cellTaker;

    public void Remove()
    {
        _cellTaker.Take().ForEach( cell => MapBasedOnTilemap.Instance._tilemap.SetTile(cell._coordinates.ToVector3Int(), null));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Remove();
    }
}
