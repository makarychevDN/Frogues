using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
                
        public static Map Instance;

        public int sizeX, sizeY;
        public Tilemap tilemap;
        public Transform unitsCellsParent, surfacesCellsParent, wallsParent;
        public List<Cell> allCells;
        
        [SerializeField] protected Cell cellPrefab;
        [SerializeField] protected Wall wallPrefab;
        [SerializeField] protected List<UnitPosition> unitsStartPositions;
        protected List<Transform> _cellsParents;
        public Dictionary<MapLayer, Cell[,]> layers;
        
        [SerializeField] private Tile emptySegment;
        public void InitCells()
        {
            BoundsInt bounds = tilemap.cellBounds;

            if (sizeX == 0)
                sizeX = bounds.size.x;
            if (sizeY == 0)
                sizeY = bounds.size.y;
            layers = new Dictionary<MapLayer, Cell[,]>();

            for (int k = 0; k < _cellsParents.Count; k++)
            {
                layers.Add((MapLayer) k, new Cell[sizeX, sizeY]);

                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        if (tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                        {
                            var instantiatedCell = Instantiate(cellPrefab,
                                tilemap.CellToWorld(new Vector3Int(i, j, 0)), Quaternion.identity);
                            layers[(MapLayer) k][i, j] = instantiatedCell;
                            instantiatedCell.transform.SetParent(_cellsParents[k]);
                            instantiatedCell.coordinates = new Vector2Int(i, j);
                            instantiatedCell.mapLayer = (MapLayer) k;
                        }
                    }
                }
                
                for (int i = 1; i < sizeX - 1; i++)
                {
                    for (int j = 1; j < sizeY - 1; j++)
                    {
                        layers[(MapLayer) k][i,j].GetComponent<HexagonCellNeighbours>().Init();
                    }
                }
            }

            allCells = new List<Cell>();

            foreach (var layer in layers)
            {
                foreach (var cell in layer.Value)
                {
                    allCells.Add(cell);
                }
            }
        }
        
        public void InitWalls()
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (tilemap.GetTile(new Vector3Int(i, j)) == emptySegment)
                    {
                        var wall = Instantiate(wallPrefab, wallsParent);
                        layers[MapLayer.DefaultUnit][i, j].Content = wall;
                        wall.transform.position = tilemap.CellToWorld(new Vector3Int(i, j, 0));
                    }
                }
            }
        }

        public void Init()
        {
            Instance = this;
            InitCellsParents();
            InitCells();
            InitWalls();
            InitCellsMonitoringOnUnitsLayer();
            InitUnitsPositionsOnMap();
        }

        public void InitCellsParents() => _cellsParents = new List<Transform> {unitsCellsParent, surfacesCellsParent};
        
        public virtual void InitCellsMonitoringOnUnitsLayer()
        {
            for (int i = 0; i < layers[0].GetLength(0); i++)
            {
                for (int j = 0; j < layers[0].GetLength(1); j++)
                {
                    if (tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                    {
                        var activateTriggerInSurfaces = layers[MapLayer.Surface][i, j]
                            .GetComponent<ActivateTriggerOnUnitsLayerCellFilled>();
                        layers[MapLayer.DefaultUnit][i, j].OnBecameFull
                            .AddListener(activateTriggerInSurfaces.TriggerOnBecameFull);
                        layers[MapLayer.DefaultUnit][i, j].OnBecameEmpty
                            .AddListener(activateTriggerInSurfaces.TriggerOnBecameEmpty);
                    }
                }
            }
        }

        public void InitUnitsPositionsOnMap()
        {
            foreach (var unitPos in unitsStartPositions)
            {
                layers[unitPos.unit.unitType][unitPos.position.x, unitPos.position.y].Content = unitPos.unit;
                unitPos.unit.transform.position = unitPos.unit.CurrentCell.transform.position;
            }
        }

        public virtual Cell FindNeighborhoodForCell(Cell startCell, Vector2Int direction)
        {
            return GetLayerByCell(startCell)[startCell.coordinates.x + direction.x,
                startCell.coordinates.y + direction.y];
        }

        public virtual Cell[,] GetLayerByCell(Cell cell)
        {
            return layers[cell.mapLayer];
        }

        public virtual Cell[,] GetLayerByType(MapLayer mapLayer)
        {
            return layers[mapLayer];
        }

        public virtual Cell GetUnitsLayerCellByCoordinates(Vector2Int coordinates)
        {
            return layers[MapLayer.DefaultUnit][coordinates.x, coordinates.y];
        }

        public virtual List<Cell> GetCellsColumn(Cell cell) => GetCellsColumn(cell.coordinates);

        public virtual List<Cell> GetCellsColumn(Vector2Int coordinates)
        {
            List<Cell> cells = new List<Cell>();
            foreach (var layer in layers)
            {
                cells.Add(layer.Value[coordinates.x, coordinates.y]);
            }

            return cells;
        }

        public virtual List<Cell> GetCellsColumnIgnoreSurfaces(Vector2Int coordinates)
        {
            var temp = GetCellsColumn(coordinates);
            temp.Remove(layers[MapLayer.Surface][coordinates.x, coordinates.y]);
            return temp;
        }

        public virtual Cell GetCell(Vector2Int coordinates, MapLayer mapLayer)
        {
            return layers[mapLayer][coordinates.x, coordinates.y];
        }
    }
    
    [Serializable]
    public struct UnitPosition
    {
        public Unit unit;
        public Vector2Int position;

    }
}
