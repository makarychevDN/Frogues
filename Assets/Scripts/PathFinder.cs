using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class PathFinder : MonoBehaviour
    {
        public Map map;
        [SerializeField] private bool isMapHexagon;
        [SerializeField] private LineRenderer lineRendererPrefab;
        [SerializeField] private Transform linesParent;
        private List<PathFinderNode> _currentNodes;
        private List<PathFinderNode> _childNodes;
        private PathFinderNode[,] _nodesGrid;
        private List<Vector2Int> _dirVectors;
        private List<Vector2Int> _additionalOddDirVectorsForHexMap;
        private List<Vector2Int> _additionalEvenDirVectorsForHexMap;
        private List<Vector2Int> _tempListForAdditionDirVectors;

        public void Init()
        {
            InitializeDirVectors();
            InitializeNodesGrid();
            FindAllNodesNeighbors();
            DrawLinesBetweenNeighbors();
        }

        private void DrawLinesBetweenNeighbors()
        {
            foreach (var node in _nodesGrid)
            {
                if (node == null)
                    continue;

                foreach (var neighbor in node.neighbors)
                {
                    if (neighbor == null)
                        continue;

                    var line = Instantiate(lineRendererPrefab, linesParent);
                    line.SetPosition(0, node.cell.transform.position);
                    line.SetPosition(1, neighbor.cell.transform.position);
                }
            }
        }

        public void ResetNodes()
        {
            foreach (var item in _nodesGrid)
            {
                if (item == null)
                    continue;

                item.usedToPathFinding = false;
                item.weight = 0;
            }
        }

        public List<Cell> FindWay(Cell userCell, Cell targetCell, bool ignoreDefaultUnits, bool ignoreSmallUnits,
            bool ignoreSurfaces)
        {
            if (userCell == null || targetCell == null)
                return null;

            ResetNodes();
            return AStar(userCell, targetCell, ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces);
        }
        
        public List<Cell> FindWayExcludeLastCell(Cell userCell, Cell targetCell, bool ignoreDefaultUnits, bool ignoreSmallUnits,
            bool ignoreSurfaces)
        {
            ResetNodes();
            var path = FindWay(userCell, targetCell, ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces);

            if (path == null)
                return null;
            
            path.RemoveAt(path.Count - 1);
            return path;
        }


        public List<Cell> GetCellsAreaByActionPoints(Cell userCell, int actionPoints, int movementCost,
            bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            ResetNodes();
            return WaveAlgorithm(userCell, movementCost == 0 ? 100 : actionPoints / movementCost, ignoreDefaultUnits,
                ignoreSmallUnits, ignoreSurfaces);
        }

        public List<Cell> GetCellsAreaByRange(Cell userCell, int range,
            bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            ResetNodes();
            return WaveAlgorithm(userCell, range, ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces);
        }

        public List<Cell> GetCellsAreaForAOE(Cell userCell, int radius, bool includeBusyCell, bool diagonalStep)
        {
            ResetNodes();
            return WaveAlgorithmForAOEWeapon(userCell, radius, includeBusyCell, diagonalStep);
        }

        private List<Cell> AStar(Cell userCell, Cell targetCell, bool ignoreDefaultUnits, bool ignoreSmallUnits,
            bool ignoreSurfaces)
        {
            _currentNodes = new List<PathFinderNode>();
            _currentNodes.Add(_nodesGrid[userCell.coordinates.x, userCell.coordinates.y]);
            _nodesGrid[userCell.coordinates.x, userCell.coordinates.y].usedToPathFinding = true;
            PathFinderNode smallestWeightNode;

            while (_currentNodes.Count != 0)
            {
                smallestWeightNode = _currentNodes[0];

                foreach (var item in _currentNodes)
                {
                    if (!item.CheckIsBusy(ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces) &&
                        item.weight < smallestWeightNode.weight)
                        smallestWeightNode = item;
                }

                foreach (var item in smallestWeightNode.neighbors)
                {

                    if (item.coordinates == new Vector2Int(targetCell.coordinates.x, targetCell.coordinates.y))
                    {
                        item.previous = smallestWeightNode;
                        List<Cell> path = new List<Cell>();
                        var tempBackTrackNode = item;
                        Cell[,] currentLayer = EntryPoint.Instance.Map.CellsArray;

                        while (tempBackTrackNode.coordinates !=
                               new Vector2Int(userCell.coordinates.x, userCell.coordinates.y))
                        {
                            path.Insert(0,
                                currentLayer[tempBackTrackNode.coordinates.x, tempBackTrackNode.coordinates.y]);
                            tempBackTrackNode = tempBackTrackNode.previous;
                        }

                        return path;
                    }
                    else if (!item.usedToPathFinding &&
                             !item.CheckIsBusy(ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces))
                    {
                        _currentNodes.Add(item);
                        item.weight = Vector3.Distance(item.cell.transform.position, userCell.transform.position) +
                                      Vector3.Distance(item.cell.transform.position, targetCell.transform.position) +
                                      item.ColumnContentWightModifiers;
                        item.previous = smallestWeightNode;
                        item.usedToPathFinding = true;
                    }
                }

                _currentNodes.Remove(smallestWeightNode);
                smallestWeightNode.usedToPathFinding = true;
            }

            return null;
        }

        private List<Cell> WaveAlgorithm(Cell userCell, int actionPoints, bool ignoreDefaultUnits,
            bool ignoreSmallUnits, bool ignoreSurfaces)
        {
            _childNodes = new List<PathFinderNode>();
            _currentNodes = new List<PathFinderNode>();
            _currentNodes.Add(_nodesGrid[userCell.coordinates.x, userCell.coordinates.y]);
            _nodesGrid[userCell.coordinates.x, userCell.coordinates.y].usedToPathFinding = true;
            int stepCounter = 0;
            List<Cell> resultCells = new List<Cell>();

            while (stepCounter < actionPoints || _childNodes.Count != 0)
            {
                foreach (var parent in _currentNodes)
                {
                    foreach (var child in parent.neighbors)
                    {
                        if (!child.usedToPathFinding &&
                            !child.CheckIsBusy(ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces))
                        {
                            child.previous = parent;
                            _childNodes.Add(child);
                            child.usedToPathFinding = true;
                            resultCells.Add(child.cell);
                        }
                    }
                }

                _currentNodes = _childNodes;
                _childNodes = new List<PathFinderNode>();
                stepCounter++;
            }

            return resultCells;
        }

        private List<Cell> WaveAlgorithmForAOEWeapon(Cell userCell, int actionPoints, bool ignoreBusyCells, bool diagonalStep)
        {
            _childNodes = new List<PathFinderNode>();
            _currentNodes = new List<PathFinderNode>();
            _currentNodes.Add(_nodesGrid[userCell.coordinates.x, userCell.coordinates.y]);
            _nodesGrid[userCell.coordinates.x, userCell.coordinates.y].usedToPathFinding = true;
            int stepCounter = 0;
            List<Cell> resultCells = new List<Cell>();

            while (stepCounter < actionPoints || _childNodes.Count != 0)
            {
                if (!diagonalStep)
                {
                    foreach (var parent in _currentNodes)
                    {
                        foreach (var child in parent.neighbors)
                        {
                            if (child.IsWall || child.usedToPathFinding)
                                continue;

                            if (child.Busy && !ignoreBusyCells)
                                continue;

                            child.previous = parent;
                            _childNodes.Add(child);
                            child.usedToPathFinding = true;
                            resultCells.Add(child.cell);
                        }
                    }
                }
                else
                {
                    PathFinderNode child;

                    foreach (var parent in _currentNodes)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (i == 0 && j == 0)
                                    continue;

                                child = _nodesGrid[parent.coordinates.x + i, parent.coordinates.y + j];

                                if (child.IsWall || child.usedToPathFinding)
                                    continue;

                                if (child.Busy && !ignoreBusyCells)
                                    continue;

                                child.previous = parent;
                                _childNodes.Add(child);
                                child.usedToPathFinding = true;
                                resultCells.Add(child.cell);
                            }
                        }
                    }
                }

                _currentNodes = _childNodes;
                _childNodes = new List<PathFinderNode>();
                stepCounter++;
            }

            return resultCells;
        }

        #region initPathfinder

        private void InitializeDirVectors()
        {
            _dirVectors = new List<Vector2Int>();
            _dirVectors.Add(Vector2Int.up);
            _dirVectors.Add(Vector2Int.right);
            _dirVectors.Add(Vector2Int.down);
            _dirVectors.Add(Vector2Int.left);
            
            if(!isMapHexagon)
                return;

            _additionalOddDirVectorsForHexMap = new List<Vector2Int>();
            _additionalOddDirVectorsForHexMap.Add(new Vector2Int(1, 1));
            _additionalOddDirVectorsForHexMap.Add(new Vector2Int(1, -1));
            _additionalEvenDirVectorsForHexMap = new List<Vector2Int>();
            _additionalEvenDirVectorsForHexMap.Add(new Vector2Int(-1, 1));
            _additionalEvenDirVectorsForHexMap.Add(new Vector2Int(-1, -1));
        }

        private void InitializeNodesGrid()
        {
            _nodesGrid = new PathFinderNode[map.sizeX, map.sizeZ];

            for (int i = 0; i < map.sizeX; i++)
            {
                for (int j = 0; j < map.sizeZ; j++)
                {
                    if (map.CellsArray[i, j] == null)
                        continue;

                    _nodesGrid[i, j] = new PathFinderNode(map.CellsArray[i, j]);
                }
            }

        }

        private void FindAllNodesNeighbors()
        {
            foreach (var node in _nodesGrid)
            {
                if(node == null) continue;

                foreach (var dir in _dirVectors)
                {
                    if (AddNeighborIsPossible(node, dir))
                        node.AddNeighbor(_nodesGrid[node.cell.coordinates.x + dir.x, node.cell.coordinates.y + dir.y]);                    
                }
                
                if(!isMapHexagon)
                    continue;

                _tempListForAdditionDirVectors = node.cell.coordinates.y.Even()
                    ? _additionalEvenDirVectorsForHexMap
                    : _additionalOddDirVectorsForHexMap;

                foreach (var dir in _tempListForAdditionDirVectors)
                {
                    if (AddNeighborIsPossible(node, dir))
                        node.AddNeighbor(_nodesGrid[node.cell.coordinates.x + dir.x, node.cell.coordinates.y + dir.y]);
                }
            }
        }

        private bool AddNeighborIsPossible(PathFinderNode node, Vector2Int dir)
        {
            if (node == null) return false;

            return node.cell.coordinates.x + dir.x > 0
                   && node.cell.coordinates.x + dir.x < _nodesGrid.GetLength(0)
                   && node.cell.coordinates.y + dir.y > 0
                   && node.cell.coordinates.y + dir.y < _nodesGrid.GetLength(1);
        }

        #endregion
    }

    public class PathFinderNode
    {
        public Cell cell;
        public Vector2Int coordinates;
        public List<PathFinderNode> neighbors;
        public bool usedToPathFinding;
        public PathFinderNode previous;
        public float weight;

        public bool CheckIsBusy(bool ignoreDefaultUnits, bool ignoreSmallUnits, bool ignoreSurfaces)
            => !cell.CheckColumnIsEmpty(ignoreDefaultUnits, ignoreSmallUnits, ignoreSurfaces) /* && !IsWall*/;

        public bool Busy => !cell.AbleToStepOnIt;

        public bool IsWall => cell.Content as Barrier;

        public PathFinderNode(Cell cell)
        {
            this.cell = cell;
            neighbors = new List<PathFinderNode>();
            coordinates = this.cell.coordinates;
        }

        public void AddNeighbor(PathFinderNode neighbor)
        {
            neighbors.Add(neighbor);
        }

        public float ColumnContentWightModifiers
        {
            get => 0;
        }
    }
}