using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBasedOnTilemap : Map
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Tilemap _tilemap;

    public List<Cell[,]> _layers;
    public Cell[,] _projectilesLayer, _unitsLayer, _surfacesLayer;

    private List<Transform> _cellsParents;
    public Transform _projectilesCellsParent, _unitsCellsParent, _surfacesCellsParent;
    private const int _layersCount = 3;

    private void Awake()
    {
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
                    }
                }
            }
        }
    }
    
    public Cell FindNeigborhoodForCell(Cell startCell, Vector2Int direction)
    {
        for (int k = 0; k < _layers.Count; k++)
        {
            for (int i = 0; i < _layers[k].GetLength(0); i++)
            {
                for (int j = 0; j < _layers[k].GetLength(1); j++)
                {
                    if (_layers[k][i, j] == startCell)
                    {
                        return _layers[k][i + direction.x, j + direction.y];
                    }
                }
            }
        }


        return null;
    }

    public Cell GetCellInDefaultUnitsLayerByUnit(Unit unit)
    {
        Cell[,] tempLayer = null;

        switch (unit._unitType)
        {
            case UnitType.DefaultUnit: return unit.currentCell;
            case UnitType.Projectile: tempLayer = _projectilesLayer; break;
            case UnitType.Surface: tempLayer = _surfacesLayer; break;
        }

        for (int i = 0; i < tempLayer.GetLength(0); i++)
        {
            for (int j = 0; j < tempLayer.GetLength(1); j++)
            {
                if(unit == tempLayer[i, j])
                {
                    return _unitsLayer[i, j];
                }
            }
        }


        return null;
    }
}
