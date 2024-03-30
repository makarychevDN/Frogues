using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
        public int sizeX, sizeZ;
        public Transform wallsParent;
        [SerializeField] public List<Cell> allCells;
        [SerializeField] public List<Cell> walls;
        [SerializeField] public Tilemap globalTilemap;
        
        protected List<Transform> _cellsParents;
        private Cell[,] _cellsArray;
        private List<Cell> _cellsAndWalls = new();

        public Cell[,] CellsArray => _cellsArray;

        public void Init(List<Room> rooms)
        { 
            CollectCellsAndWallsFromRooms(rooms);

            sizeX = _cellsAndWalls.Max(walls => walls.Coordinates.x) + 1;
            sizeZ = _cellsAndWalls.Max(walls => walls.Coordinates.y) + 1;
            _cellsArray = new Cell[sizeX, sizeZ];

            foreach(Cell cell in _cellsAndWalls)
            {
                _cellsArray[cell.Coordinates.x, cell.Coordinates.y] = cell;
            }

            foreach(Cell cell in allCells)
            {
                cell.CellNeighbours.Init();
            }

            globalTilemap.GetComponent<TilemapRenderer>().enabled = false;
        }

        private void CollectCellsAndWallsFromRooms(List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                room.TurnOffTileMapRenderer();

                foreach (Cell cell in room.AllCells)
                {
                    allCells.Add(cell);
                    _cellsAndWalls.Add(cell);
                    cell.Coordinates = globalTilemap.WorldToCell(cell.transform.position).ToVector2Int();
                }

                foreach (Cell wall in room.Walls)
                {
                    walls.Add(wall);
                    _cellsAndWalls.Add(wall);
                    wall.Coordinates = globalTilemap.WorldToCell(wall.transform.position).ToVector2Int();
                }
            }
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
