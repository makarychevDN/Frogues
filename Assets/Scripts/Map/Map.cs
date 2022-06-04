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
        protected List<Transform> _cellsParents;
        public Dictionary<MapLayer, Cell[,]> layers;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            InitCellsParents();
            InitCells();
            InitWalls();
            InitCellsMonitoringOnUnitsLayer();
        }

        public void InitCellsParents() => _cellsParents = new List<Transform> {unitsCellsParent, surfacesCellsParent};

        public virtual void InitCells()
        {
            BoundsInt bounds = tilemap.cellBounds;
            
            if(sizeX == 0)
                sizeX = bounds.size.x;
            if(sizeY == 0)
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
                                tilemap.CellToWorld(new Vector3Int(i, j, 0)) + Vector3.up * 0.5f, Quaternion.identity);
                            layers[(MapLayer) k][i, j] = instantiatedCell;
                            instantiatedCell.transform.SetParent(_cellsParents[k]);
                            instantiatedCell.coordinates = new Vector2Int(i, j);
                            instantiatedCell.mapLayer = (MapLayer) k;
                        }
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

        public virtual void InitWalls()
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if ((i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1))
                    {
                        var wall = Instantiate(wallPrefab, wallsParent);
                        layers[MapLayer.DefaultUnit][i, j].Content = wall;
                        wall.transform.position = layers[MapLayer.DefaultUnit][i, j].coordinates.ToVector3();
                    }
                }
            }
        }

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
}