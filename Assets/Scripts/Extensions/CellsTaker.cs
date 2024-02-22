using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

namespace FroguesFramework
{
    public static class CellsTaker
    {
        public static List<Cell> TakeCellsAreaByRange(Cell startCell, int radius, bool ignoreBusyCell = true)
        {
            return EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(startCell, radius, ignoreBusyCell, false);
        }

        public static List<Cell> TakeCellsLineInDirection(Cell startCell, HexDir direction, ObstacleMode obstacleMode, bool includeFirstCellWithObstacle, bool lineStopsWithObstacle)
        {
            List<Cell> cells = new List<Cell>();
            Cell currentCell = startCell;
            bool isTheFirstObstacleInARow = true;

            while (true)
            {
                currentCell = currentCell.CellNeighbours.GetNeighborByHexDir(direction);

                if (currentCell.Content is Barrier)
                    break;

                if (IsCellContainsObstacle(currentCell, obstacleMode))
                {
                    if (includeFirstCellWithObstacle && isTheFirstObstacleInARow)
                    {
                        cells.Add(currentCell);
                        isTheFirstObstacleInARow = false;
                    }

                    if (lineStopsWithObstacle)
                    {
                        break;
                    }

                    continue;
                }

                cells.Add(currentCell);
                isTheFirstObstacleInARow = true;
            }

            return cells;
        }

        public static List<Cell> GetBestCellsToRetreatFromTarget(Unit retreater, Unit target)
        {
            var theBestCellsToRetreat = new List<Cell>() { retreater.CurrentCell };
            var neighborCells = CellsTaker.TakeCellsAreaByRange(retreater.CurrentCell, 1).EmptyCellsOnly();
            var farestDistance = target.CurrentCell.DistanceToCell(retreater.CurrentCell);

            foreach (var cell in neighborCells)
            {
                if (target.CurrentCell.DistanceToCell(cell) > farestDistance)
                {
                    theBestCellsToRetreat.Clear();
                    farestDistance = target.CurrentCell.DistanceToCell(cell);
                }

                if (target.CurrentCell.DistanceToCell(cell) == farestDistance)
                    theBestCellsToRetreat.Add(cell);
            }

            if (theBestCellsToRetreat.Contains(retreater.CurrentCell) && neighborCells.Where(cell => cell.Content == target).Count() > 0)
            {
                int leastBarriersQuantity = 6;

                foreach (var cell in neighborCells)
                {
                    int barriersQuantity = cell.CellNeighbours.GetAllNeighbors().Where(cell => cell.Content is Barrier).Count();

                    if (barriersQuantity < leastBarriersQuantity)
                    {
                        leastBarriersQuantity = barriersQuantity;
                        theBestCellsToRetreat.Clear();
                        farestDistance = target.CurrentCell.DistanceToCell(cell);
                    }

                    if (target.CurrentCell.DistanceToCell(cell) == farestDistance)
                        theBestCellsToRetreat.Add(cell);
                }
            }

            return theBestCellsToRetreat;
        }

        private static bool IsCellContainsObstacle(Cell cell, ObstacleMode obstacleMode)
        {
            if (cell.Content is Barrier)
                return true;

            if (cell.IsEmpty || obstacleMode == ObstacleMode.noObstacles)
                return false;

            if(obstacleMode == ObstacleMode.everyUnitIsObstacle)
                return true;

            return !cell.Content.Small;
        }

        public enum ObstacleMode
        {
            noObstacles = 10, onlyBigUnitsAreObstacles = 20, everyUnitIsObstacle = 30
        }

        public static List<Cell> TakeCellsLinesInAllDirections(Cell startCell, ObstacleMode obstacleMode, bool includeFirstCellWithObstacle, bool lineStopsWithObstacle)
        {
            List<Cell> cells = new List<Cell>();

            foreach (var hexDir in Enum.GetValues(typeof(HexDir)).Cast<HexDir>())
            {
                cells.AddRange(TakeCellsLineInDirection(startCell, hexDir, obstacleMode, includeFirstCellWithObstacle, lineStopsWithObstacle));   
            }

            return cells;
        }

        public static HexDir GetDirByStartAndEndCells(Cell startCell, Cell endCell)
        {
            List<Cell> cells = new List<Cell>();

            foreach (var hexDir in Enum.GetValues(typeof(HexDir)).Cast<HexDir>())
            {
                cells.AddRange(TakeCellsLineInDirection(startCell, hexDir,ObstacleMode.noObstacles, false, false));

                if (cells.Contains(endCell))
                {
                    return hexDir;
                }
            }

            return (Enum.Parse<HexDir>("exception"));
        }
        
        public static List<Cell> TakeAllCells()
        {
            return EntryPoint.Instance.Map.allCells;
        }

        public static List<Cell> TakeAllEmptyCells()
        {
            return EntryPoint.Instance.Map.allCells.Where(cell => cell.IsEmpty).ToList();
        }

        public static List<Unit> TakeAllUnits()
        {
            var cellsWithContent = EntryPoint.Instance.Map.allCells.Where(cell => !cell.IsEmpty).ToList();
            List<Unit> units = new List<Unit>();
            cellsWithContent.ForEach(cell => units.Add(cell.Content));
            return units;
        }

        public static List<Cell> TakeCellsLineWhichContainCell(Cell startCell, Cell targetCell, ObstacleMode obstacleMode, bool includeFirstCellWithObstacle, bool lineStopsWithObstacle)
        {
            foreach (var hexDir in Enum.GetValues(typeof(HexDir)).Cast<HexDir>())
            {
                var line = TakeCellsLineInDirection(startCell, hexDir, obstacleMode, includeFirstCellWithObstacle, lineStopsWithObstacle);
                if (line.Contains(targetCell))
                    return line;
            }

            return new List<Cell>();
        }
        
        public static List<Cell> EmptyCellsOnly(this List<Cell> cells)
        {
            return cells.Where(cell => cell.IsEmpty).ToList();
        }

        public static List<Cell> AbleToStepCellsOnly(this List<Cell> cells)
        {
            return cells.Where(cell => cell.AbleToStepOnIt).ToList();
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

        public static Unit TakeBloodSurfaceByMouseRaycast()
        {
            var mask = LayerMask.GetMask("UI", "Blood Surface");
            RaycastHit hit;

            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask)
                ? hit.transform.GetComponentInParent<Unit>()
                : null;
        }

        public static Unit TakeUnitByLayersWithMouseRaycast(List<string> layers)
        {
            layers.Add("UI");
            var mask = LayerMask.GetMask(layers.ToArray());
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

        public static Cell GetCellBeforeOtherCellInDirection(Cell startCell, Cell otherCell)
        {
            List<Cell> line = TakeCellsLineWhichContainCell(startCell, otherCell, ObstacleMode.noObstacles, false, false);
            
            for(int i = 0; i < line.Count; i++)
            {
                if (line[i] == otherCell)
                    return line[i - 1];
            }

            return null;
        }
    }
}