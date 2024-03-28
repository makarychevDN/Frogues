using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
        public int sizeX, sizeZ;
        public UnityEngine.Transform unitsCellsParent, surfacesCellsParent, wallsParent;
        [SerializeField] public List<Cell> allCells;
        [SerializeField] public Tilemap tilemap;
        
        [SerializeField] protected Cell wallPrefab;
        [SerializeField] protected List<Cell> cellsPrefabs;
        protected List<UnityEngine.Transform> _cellsParents;
        private Cell[,] _cellsArray;

        public Cell[,] CellsArray => _cellsArray;

        private TileBase GetTileFromListByCoordinates(TileBase[] allTiles, BoundsInt bounds, int x, int y) => allTiles[x + y * bounds.size.x];

        private bool IsNullTileNearby(TileBase[] allTiles, BoundsInt bounds, int x, int y)
        {
            try
            {
                int evenModificator = y.Even().ToInt();
                int oddModificator = y.Odd().ToInt();

                var topLeftTile = GetTileFromListByCoordinates(allTiles, bounds, x - evenModificator, y + 1);
                var topRightTile = GetTileFromListByCoordinates(allTiles, bounds, x + oddModificator, y + 1);

                var bottomLeftTile = GetTileFromListByCoordinates(allTiles, bounds, x - evenModificator, y - 1);
                var bottomRightTile = GetTileFromListByCoordinates(allTiles, bounds, x + oddModificator, y - 1);

                var leftTile = GetTileFromListByCoordinates(allTiles, bounds, x - 1, y);
                var rightTile = GetTileFromListByCoordinates(allTiles, bounds, x + 1, y);

                return topLeftTile == null || topRightTile == null || bottomLeftTile == null || bottomRightTile == null || leftTile == null || rightTile == null;
            }

            catch
            {
                return true;
            }
        }

        public void Init()
        { 
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

                        if (IsNullTileNearby(allTiles, bounds, x, y))
                        {
                            spawnedCell = Instantiate(wallPrefab, wallsParent);
                        }
                        else
                        {
                            spawnedCell = Instantiate(cellsPrefabs.GetRandomElement(), transform);
                            allCells.Add(spawnedCell);
                        }

                        _cellsArray[x, y] = spawnedCell;
                        spawnedCell.coordinates = new Vector2Int(x, y);
                        spawnedCell.transform.position = tilemap.CellToWorld(new Vector3Int(x, y));
                    }
                }
            }

            allCells.ForEach(cell => cell.CellNeighbours.Init());
            tilemap.GetComponent<TilemapRenderer>().enabled = false;
        }

        public virtual Cell GetCell(Vector2Int coordinates)
        {
            return _cellsArray[coordinates.x, coordinates.y];
        }
        
        public void SetCell(Cell hexCell3D)
        {
            allCells.RemoveAll(cell => cell == null);
            
            if(!allCells.Contains(hexCell3D))
                allCells.Add(hexCell3D);
        }
        
        public void RemoveCell(Cell hexCell3D)
        {
            allCells.RemoveAll(cell => cell == null);
            
            if(allCells.Contains(hexCell3D))
                allCells.Remove(hexCell3D);
        }
    }
}
