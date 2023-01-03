using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public static class CellsTaker
    {
        public static List<Cell> TakeCellsAreaByRange(Cell startCell, int radius)
        {
            return PathFinder.Instance.GetCellsAreaForAOE(startCell, radius, true, false);
        }

        public static List<Cell> TakeCellsLineInDirection(Cell startCell, HexDir direction)
        {
            List<Cell> cells = new List<Cell>();
            Cell currentCell = startCell;

            while (currentCell.CellNeighbours.GetNeighborByHexDir(direction).Content == null)
            {
                currentCell = currentCell.CellNeighbours.GetNeighborByHexDir(direction);
                cells.Add(currentCell);
            }

            return cells;
        }
        
        public static List<Cell> TakeCellsLinesInAllDirections(Cell startCell)
        {
            List<Cell> cells = new List<Cell>();

            foreach (var hexDir in Enum.GetValues(typeof(HexDir)).Cast<HexDir>())
            {
                cells.AddRange(TakeCellsLineInDirection(startCell, hexDir));   
            }

            return cells;
        }

        public static List<Cell> TakeCellsLineWhichContainCell(Cell startCell, Cell targetCell)
        {
            foreach (var hexDir in Enum.GetValues(typeof(HexDir)).Cast<HexDir>())
            {
                if (TakeCellsLineInDirection(startCell, hexDir).Contains(targetCell))
                    return TakeCellsLineInDirection(startCell, hexDir);
            }

            return new List<Cell>();
        }

        public static Cell TakeCellByMouseRaycast()
        {
            RaycastHit hit;

            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)
                ? hit.transform.parent.GetComponent<Cell>()
                : null;
        }
    }
}