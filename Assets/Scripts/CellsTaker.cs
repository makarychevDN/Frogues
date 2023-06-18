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
            return EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(startCell, radius, true, false);
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
        
        public static List<Cell> TakeAllCells()
        {
            return EntryPoint.Instance.Map.allCells;
        }

        public static List<Cell> TakeAllEmptyCells()
        {
            return EntryPoint.Instance.Map.allCells.Where(cell => cell.IsEmpty).ToList();
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
        
        public static List<Cell> EmptyCellsOnly(this List<Cell> cells)
        {
            return cells.Where(cell => cell.IsEmpty).ToList();
        }

        public static Cell TakeCellByMouseRaycast()
        {
            var mask = LayerMask.GetMask("UI", "Cell");
            RaycastHit hit;

            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask)
                ? hit.transform.GetComponentInParent<Cell>()
                : null;
        }

        public static MonoBehaviour TakeCellOrUnitByMouseRaycast()
        {
            var mask = LayerMask.GetMask("UI", "Cell", "Unit");
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask))
            {
                var cell = hit.transform.GetComponentInParent<Cell>();
                if (cell != null)
                    return cell;

                return hit.transform.GetComponentInParent<Unit>();
            }

            return null;
        }

        public static Unit TakeUnitByMouseRaycast()
        {
            var mask = LayerMask.GetMask("UI", "Unit");
            RaycastHit hit;

            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask)
                ? hit.transform.GetComponentInParent<Unit>()
                : null;
        }

        public static Cell JumpOverNeighborCell(Cell startCell, Cell targetCell)
        {
            var hexDir = startCell.CellNeighbours.GetHexDirByNeighbor(targetCell);
            return targetCell.CellNeighbours.GetNeighborByHexDir(hexDir);
        }
    }
}