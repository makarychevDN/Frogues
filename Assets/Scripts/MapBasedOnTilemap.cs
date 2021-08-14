using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBasedOnTilemap : Map
{
    public static MapBasedOnTilemap Instance;

    [SerializeField] private Cell _cellPrefab;
    public Tilemap _tilemap;
    [SerializeField] private Unit _wallPrefab;
    public int _sizeX, _sizeY;

    public List<Cell[,]> _layers;
    public Cell[,] _projectilesLayer, _unitsLayer, _surfacesLayer;

    private List<Transform> _cellsParents;
    public Transform _projectilesCellsParent, _unitsCellsParent, _surfacesCellsParent, _wallsParent;
    private const int _layersCount = 3;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        InitCellsParents();
        InitCells();
        InitLayers();
        InitWalls();
        InitCellsMonitoringOnUnitsLayer();
    }

    public void InitCellsParents()
    {
        _cellsParents = new List<Transform>();
        _cellsParents.Add(_projectilesCellsParent);
        _cellsParents.Add(_unitsCellsParent);
        _cellsParents.Add(_surfacesCellsParent);
    }

    public void InitCells()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        _sizeX = bounds.size.x;
        _sizeY = bounds.size.y;
        _layers = new List<Cell[,]>();

        for (int k = 0; k < _layersCount; k++)
        {
            _layers.Add(new Cell[_sizeX, _sizeY]);

            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
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

    public void InitWalls()
    {
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                if ((i == 0 || j == 0 || i == _sizeX - 1 || j == _sizeY - 1))
                {
                    var wall = Instantiate(_wallPrefab, _wallsParent);
                    _unitsLayer[i, j].Content = wall;
                    wall.transform.position = _unitsLayer[i, j]._coordinates.ToVector3();
                }
            }
        }
    }

    public void InitLayers()
    {
        _projectilesLayer = _layers[0];
        _unitsLayer = _layers[1];
        _surfacesLayer = _layers[2];
    }

    public void InitCellsMonitoringOnUnitsLayer()
    {
        for (int i = 0; i < _layers[0].GetLength(0); i++)
        {
            for (int j = 0; j < _layers[0].GetLength(1); j++)
            {
                if (_tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    var activateTriggerInProjectiles = _projectilesLayer[i, j].GetComponent<ActivateTriggerOnUnitsLayerCellFilled>();
                    var activateTriggerInSurfaces = _surfacesLayer[i, j].GetComponent<ActivateTriggerOnUnitsLayerCellFilled>();

                    _unitsLayer[i, j].OnBecameFull.AddListener(activateTriggerInProjectiles.TriggerOnBecameFull);
                    _unitsLayer[i, j].OnBecameEmpty.AddListener(activateTriggerInProjectiles.TriggerOnBecameEmpty);

                    _surfacesLayer[i, j].OnBecameFull.AddListener(activateTriggerInSurfaces.TriggerOnBecameFull);
                    _surfacesLayer[i, j].OnBecameEmpty.AddListener(activateTriggerInSurfaces.TriggerOnBecameEmpty);
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
