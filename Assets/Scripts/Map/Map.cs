using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class Map : MonoBehaviour
    {
        public int sizeX, sizeZ;
        public Transform unitsCellsParent, surfacesCellsParent, wallsParent;
        [SerializeField] public List<Cell> allCells;
        
        [SerializeField] protected Cell wallPrefab;
        [SerializeField] protected List<UnitPosition> unitsStartPositions;
        protected List<Transform> _cellsParents;
        public Dictionary<MapLayer, Cell[,]> layers;

        public void Init()
        {
            allCells.ForEach(cell => cell.coordinates = GetGridPosition(cell));
            
            // +2 for cells with walls
            sizeX = allCells.Select(cell => cell.coordinates.x).Max() + 2;
            sizeZ = allCells.Select(cell => cell.coordinates.y).Max() + 2;
            
            var defaultUnitLayer = new Cell[sizeX, sizeZ];
            layers = new Dictionary<MapLayer, Cell[,]>();
            layers.Add(MapLayer.DefaultUnit, defaultUnitLayer);
            allCells.ForEach(cell => defaultUnitLayer[cell.coordinates.x, cell.coordinates.y] = cell);

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

            allCells.ForEach(cell => cell.CellNeighbours.Init());
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
            //temp.Remove(layers[MapLayer.Surface][coordinates.x, coordinates.y]);
            return temp;
        }

        public virtual Cell GetCell(Vector2Int coordinates, MapLayer mapLayer)
        {
            return layers[mapLayer][coordinates.x, coordinates.y];
        }
        
            
        public Vector2Int GetGridPosition(Cell cell) => new Vector2Int(
            Convert.ToInt32((cell.transform.localPosition.x - OddXModificator(cell)) / (GridStep.X)),
            Convert.ToInt32((cell.transform.localPosition.z) / GridStep.Z));
    
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
