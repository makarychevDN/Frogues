using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public static Map Instance;

    public int sizeX, sizeY;
    public Tilemap tilemap;
    public Transform projectilesCellsParent, unitsCellsParent, surfacesCellsParent, wallsParent;
    public Cell[,] projectilesLayer, unitsLayer, surfacesLayer;
    public List<Cell[,]> layers;
    public List<Cell> allCells;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Wall wallPrefab;
    private List<Transform> _cellsParents;
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
        _cellsParents.Add(projectilesCellsParent);
        _cellsParents.Add(unitsCellsParent);
        _cellsParents.Add(surfacesCellsParent);
    }

    public void InitCells()
    {
        BoundsInt bounds = tilemap.cellBounds;
        sizeX = bounds.size.x;
        sizeY = bounds.size.y;
        layers = new List<Cell[,]>();

        for (int k = 0; k < _layersCount; k++)
        {
            layers.Add(new Cell[sizeX, sizeY]);

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                    {
                        var instantiatedCell = Instantiate(cellPrefab, tilemap.CellToWorld(new Vector3Int(i, j, 0)) + Vector3.up * 0.5f, Quaternion.identity);
                        layers[k][i, j] = instantiatedCell;
                        instantiatedCell.transform.SetParent(_cellsParents[k]);
                        instantiatedCell.coordinates = new Vector2Int(i, j);
                        instantiatedCell.mapLayer = (MapLayer)k;
                    }
                }
            }
        }

        allCells = new List<Cell>();

        layers.ForEach(layer =>
        {
            foreach (var cell in layer)
            {
                allCells.Add(cell);
            }
        });
    }

    public void InitWalls()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if ((i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1))
                {
                    var wall = Instantiate(wallPrefab, wallsParent);
                    unitsLayer[i, j].Content = wall;
                    wall.transform.position = unitsLayer[i, j].coordinates.ToVector3();
                }
            }
        }
    }

    public void InitLayers()
    {
        projectilesLayer = layers[0];
        unitsLayer = layers[1];
        surfacesLayer = layers[2];
    }

    public void InitCellsMonitoringOnUnitsLayer()
    {
        for (int i = 0; i < layers[0].GetLength(0); i++)
        {
            for (int j = 0; j < layers[0].GetLength(1); j++)
            {
                if (tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    var activateTriggerInProjectiles = projectilesLayer[i, j].GetComponent<ActivateTriggerOnUnitsLayerCellFilled>();
                    var activateTriggerInSurfaces = surfacesLayer[i, j].GetComponent<ActivateTriggerOnUnitsLayerCellFilled>();

                    unitsLayer[i, j].OnBecameFull.AddListener(activateTriggerInProjectiles.TriggerOnBecameFull);
                    unitsLayer[i, j].OnBecameEmpty.AddListener(activateTriggerInProjectiles.TriggerOnBecameEmpty);

                    surfacesLayer[i, j].OnBecameFull.AddListener(activateTriggerInSurfaces.TriggerOnBecameFull);
                    surfacesLayer[i, j].OnBecameEmpty.AddListener(activateTriggerInSurfaces.TriggerOnBecameEmpty);
                }
            }
        }
    }

    public Cell FindNeigborhoodForCell(Cell startCell, Vector2Int direction)
    {
        return GetLayerByCell(startCell)[startCell.coordinates.x + direction.x, startCell.coordinates.y + direction.y];
    }

    public Cell[,] GetLayerByCell(Cell cell)
    {
        switch (cell.mapLayer)
        {
            case MapLayer.DefaultUnit: return unitsLayer;
            case MapLayer.Projectile: return projectilesLayer;
            case MapLayer.Surface: return surfacesLayer;
        }
        return null;
    }

    public Cell GetUnitsLayerCellByCoordinates(Vector2Int coordinates) 
    {
        return unitsLayer[coordinates.x, coordinates.y];
    }

    public List<Cell> GetCellsColumn(Vector2Int coordinates)
    {
        List<Cell> cells = new List<Cell>();
        foreach (var layer in layers)
        {
            cells.Add(layer[coordinates.x, coordinates.y]);
        }

        return cells;
    }
}
