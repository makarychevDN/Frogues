using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBasedOnTilemap : Map
{
    [SerializeField] private int _xSize;
    [SerializeField] private int _ySize;
    [SerializeField] private Cell _cell;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile _tile;

    public Cell[,] _unitsLayer;

    private void Awake()
    {
        InitMap(_xSize, _ySize);
    }

    public void InitMap(int x, int y)
    {
        BoundsInt bounds = _tilemap.cellBounds;
        _unitsLayer = new Cell[bounds.size.x, bounds.size.y];

        for (int i = 0; i < _unitsLayer.GetLength(0); i++)
        {
            for (int j = 0; j < _unitsLayer.GetLength(1); j++)
            {
                if (_tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    var instantiatedCell = Instantiate(_cell, _tilemap.CellToWorld(new Vector3Int(i, j, 0)) + Vector3.up * 0.5f, Quaternion.identity) ;
                    _unitsLayer[i, j] = instantiatedCell;
                }
            }
        }
    }
    
    public Cell FindNeigborhoodForCell(Cell startCell, Vector2Int direction)
    {
        for (int i = 0; i < _unitsLayer.GetLength(0); i++)
        {
            for (int j = 0; j < _unitsLayer.GetLength(1); j++)
            {
                if (_unitsLayer[i, j] == startCell)
                {
                    return _unitsLayer[i + direction.x, j + direction.y];
                }
            }
        }

        return null;
    }
}
