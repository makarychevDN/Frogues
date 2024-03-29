using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
        public int sizeX, sizeZ;
        public Transform wallsParent;
        [SerializeField] public List<Cell> allCells;
        [SerializeField] public Tilemap tilemap;
        
        protected List<Transform> _cellsParents;
        private Cell[,] _cellsArray;

        public Cell[,] CellsArray => _cellsArray;

        public void Init()
        { 
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            _cellsArray = new Cell[bounds.size.x, bounds.size.y];
            sizeX = bounds.size.x;
            sizeZ = bounds.size.y;

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
