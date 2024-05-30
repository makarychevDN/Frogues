using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform cellsParent;
        [SerializeField] public Tilemap tilemap;
        [SerializeField] public Tile cellTile;
        [SerializeField] public Tile wallTile;

        [Header("Cells Prefabs")]
        [SerializeField] protected Cell wallPrefab;
        [SerializeField] protected List<Cell> cellsPrefabs;

        [Header("Debug Info (do not touch)")]
        [SerializeField] private int sizeX;
        [SerializeField] private int sizeZ;
        [SerializeField] public List<Cell> allCells;

        private Cell[,] _cellsArray;

        public Cell[,] CellsArray => _cellsArray;
        public int SizeX => sizeX;
        public int SizeZ => sizeZ;

        private TileBase GetTileFromListByCoordinates(TileBase[] allTiles, BoundsInt bounds, int x, int y) => allTiles[x + y * bounds.size.x];

        public virtual Cell GetCell(Vector2Int coordinates)
        {
            return _cellsArray[coordinates.x, coordinates.y];
        }

        public virtual Cell GetCell(int x, int y)
        {
            return _cellsArray[x, y];
        }

        [ContextMenu("Switch Tilemap Renderer")]
        public void SwitchTilemapRenderer()
        {
            var tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
            tilemapRenderer.enabled = !tilemapRenderer.enabled;
        }


        [ContextMenu("Generate Cells")]
        public void GenerateCells()
        {
            RemoveAllCells();

            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            _cellsArray = new Cell[bounds.size.x, bounds.size.y];
            sizeX = bounds.size.x;
            sizeZ = bounds.size.y;

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = GetTileFromListByCoordinates(allTiles, bounds, x, y);

                    if (tile != null)
                    {
                        Cell spawnedCell;

                        if (tile == wallTile)
                        {
                            spawnedCell = Instantiate(wallPrefab, cellsParent);
                        }
                        else
                        {
                            spawnedCell = Instantiate(cellsPrefabs.GetRandomElement(), cellsParent);
                            allCells.Add(spawnedCell);
                        }

                        _cellsArray[x, y] = spawnedCell;
                        spawnedCell.coordinates = new Vector2Int(x, y);
                        spawnedCell.transform.position = tilemap.CellToWorld(new Vector3Int(x, y));
                    }
                }
            }

            allCells.ForEach(cell => cell.CellNeighbours.Init(this));
            tilemap.GetComponent<TilemapRenderer>().enabled = false;
        }

        private void RemoveAllCells()
        {
            allCells.Clear();
            var cellsGameObjects = cellsParent.GetComponentsInChildren<Cell>();
            for(int i = 0;  i < cellsGameObjects.Length; i++)
            {
                DestroyImmediate(cellsGameObjects[i].gameObject);
            }            
        }
    }
}
