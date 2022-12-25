using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
        public static Map Instance;

        public int sizeX, sizeZ;
        public Transform unitsCellsParent, surfacesCellsParent, wallsParent;
        [SerializeField] public List<Cell> allCells;
        
        [SerializeField] protected Cell cellPrefab;
        [SerializeField] protected Cell wallPrefab;
        [SerializeField] protected List<UnitPosition> unitsStartPositions;
        protected List<Transform> _cellsParents;
        public Dictionary<MapLayer, Cell[,]> layers;

        private float _lowestXPosition, _lowestZPosition;
        private float _biggestXPosition, _biggestZPosition;

        public void Init()
        {
            Instance = this;
            
            _lowestXPosition = allCells.Select(cell => cell.transform.localPosition.x).Min();
            _lowestZPosition = allCells.Select(cell => cell.transform.localPosition.z).Min();
            _biggestXPosition = allCells.Select(cell => cell.transform.localPosition.x).Max();
            _biggestZPosition = allCells.Select(cell => cell.transform.localPosition.z).Max();

            // +2 for cells with walls and +1 to escape the outOfRangeException
            sizeX = Convert.ToInt32((_biggestXPosition - _lowestXPosition) / GridStep.X) + 3;
            sizeZ = Convert.ToInt32((_biggestZPosition - _lowestZPosition) / GridStep.Z) + 3;

            var defaultUnitLayer = new Cell[sizeX, sizeZ];
            layers = new Dictionary<MapLayer, Cell[,]>();
            layers.Add(MapLayer.DefaultUnit, defaultUnitLayer);

            foreach (var cell in allCells)
            {
                cell.coordinates = GetGridPosition(cell);
                defaultUnitLayer[cell.coordinates.x, cell.coordinates.y] = cell;
            }

            for (int i = 0; i < defaultUnitLayer.GetLength(0); i++)
            {
                for (int j = 0; j < defaultUnitLayer.GetLength(1); j++)
                {
                    if (defaultUnitLayer[i, j] == null)
                    {  
                        var wall = Instantiate(wallPrefab, wallsParent);
                        var zPos = j * GridStep.Z;
                        var xPos = i * GridStep.X + j % 2 * GridStep.X * 0.5f;
                        wall.transform.localPosition = new Vector3(xPos, 0, zPos);
                        defaultUnitLayer[i, j] = wall;
                    }
                }
            }
            
            foreach (var cell in allCells)
            {
                cell.CellNeighbours.Init();
            }
        }

        #region CheckIsItLegacy
        
        
        /*public void InitUnitsPositionsOnMap()
        {
            foreach (var unitPos in unitsStartPositions)
            {
                layers[unitPos.unit.unitType][unitPos.position.x, unitPos.position.y].Content = unitPos.unit;
                unitPos.unit.transform.position = unitPos.unit.CurrentCell.transform.position;
            }
        }*/
        
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

        //public virtual List<Cell> GetCellsColumn(Cell cell) => GetCellsColumn(cell.coordinates);
        
        #endregion

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
            print("I trying to get cell in " + coordinates);
            print("Map Size Is " + sizeX + " " + sizeZ);
            return layers[mapLayer][coordinates.x, coordinates.y];
        }
        
            
        private Vector2Int GetGridPosition(Cell cell) => new Vector2Int(
            Convert.ToInt32((cell.transform.localPosition.x - _lowestXPosition - OddXModificator(cell)) / (GridStep.X) + 1),
            Convert.ToInt32((cell.transform.localPosition.z - _lowestZPosition) / GridStep.Z) + 1);
    
        private float OddXModificator(Cell cell)
        {
            return cell.transform.localPosition.z / GridStep.Z % 2 * GridStep.X / 2;
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
    
    [Serializable]
    public struct UnitPosition
    {
        public Unit unit;
        public Vector2Int position;

    }
}
