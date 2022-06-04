using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class HexMap : Map
    {
        public override void InitCells()
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
        
        public override void InitWalls()
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if ((i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1))
                    {
                        var wall = Instantiate(wallPrefab, wallsParent);
                        layers[MapLayer.DefaultUnit][i, j].Content = wall;
                        wall.transform.position = tilemap.CellToWorld(new Vector3Int(i, j, 0));
                    }
                }
            }
        }
    }
}
