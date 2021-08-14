using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTilesByCells : MonoBehaviour
{
    [SerializeField] private BaseCellsTaker _cellTaker;

    public void Remove()
    {
        foreach(var cell in _cellTaker.Take())
        {
            MapBasedOnTilemap.Instance._tilemap.SetTile(cell._coordinates.ToVector3Int(), null);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Remove();
    }
}
