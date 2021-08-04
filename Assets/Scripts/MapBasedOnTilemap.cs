using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBasedOnTilemap : Map
{
    public static MapBasedOnTilemap _instance;

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Tilemap _tilemap;

    public List<Cell[,]> _layers;
    public Cell[,] _projectilesLayer, _unitsLayer, _surfacesLayer;

    private List<Transform> _cellsParents;
    public Transform _projectilesCellsParent, _unitsCellsParent, _surfacesCellsParent;
    private const int _layersCount = 3;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        _layers = new List<Cell[,]>();

        _cellsParents = new List<Transform>();
        _cellsParents.Add(_projectilesCellsParent);
        _cellsParents.Add(_unitsCellsParent);
        _cellsParents.Add(_surfacesCellsParent);

        InitMap();

        _projectilesLayer = _layers[0];
        _unitsLayer = _layers[1];
        _surfacesLayer = _layers[2];
    }

    public void InitMap()
    {
        BoundsInt bounds = _tilemap.cellBounds;

        for (int k = 0; k < _layersCount; k++)
        {
            _layers.Add(new Cell[bounds.size.x, bounds.size.y]);

            for (int i = 0; i < _layers[k].GetLength(0); i++)
            {
                for (int j = 0; j < _layers[k].GetLength(1); j++)
                {
                    if (_tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                    {
                        var instantiatedCell = Instantiate(_cellPrefab, _tilemap.CellToWorld(new Vector3Int(i, j, 0)) + Vector3.up * 0.5f, Quaternion.identity);
                        _layers[k][i, j] = instantiatedCell;
                        instantiatedCell.transform.SetParent(_cellsParents[k]);
                        instantiatedCell._coordinates = new Vector2Int(i, j);
                        instantiatedCell._mapLayer = (MapLayer)k;
                    }
                }
            }
        }
    }

    public Cell FindNeigborhoodForCell(Cell startCell, Vector2Int direction)
    {
        return GetLayerByCell(startCell)[startCell._coordinates.x + direction.x, startCell._coordinates.y + direction.y];
    }

    public Cell[,] GetLayerByCell(Cell cell)
    {
        switch (cell._mapLayer)
        {
            case MapLayer.DefaultUnit: return _unitsLayer;
            case MapLayer.Projectile: return _projectilesLayer;
            case MapLayer.Surface: return _surfacesLayer;
        }
        return null;
    }

    public Cell GetUnitsLayerCellByCoordinates(Vector2Int coordinates) 
    {
        return _unitsLayer[coordinates.x, coordinates.y];
    }

    public List<Cell> GetCellsColumn(Vector2Int coordinates)
    {
        List<Cell> cells = new List<Cell>();
        foreach (var layer in _layers)
        {
            cells.Add(layer[coordinates.x, coordinates.y]);
        }

        return cells;
    }
}
