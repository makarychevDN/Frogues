using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance;
    public Map map;
    private List<PathFinderNode> _currentNodes;
    private List<PathFinderNode> _childNodes;
    private PathFinderNode[,] _nodesGrid;
    private List<Vector2Int> _dirVectors;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        InitializeDirVectors();
        InitializeNodesGrid();
        FindAllNodesNeighbors();
    }

    public void ResetNodes()
    {
        foreach (var item in _nodesGrid)
        {
            item.usedToPathFinding = false;
            item.weight = 0;
        }
    }

    public List<Cell> FindWay(Cell userCell, Cell targetCell)
    {
        ResetNodes();
        return AStar(userCell, targetCell);
    }

    public List<Cell> GetCellsAreaByActionPoints(Cell userCell, int actionPoints, int movemetCost)
    {
        ResetNodes();
        return WaveAlgorithm(userCell, movemetCost == 0 ? 100 : actionPoints / movemetCost);
    }

    public List<Cell> GetCellsAreaForAOE(Cell userCell, int radius, bool ignoreBusyCell, bool diagonalStep)
    {
        ResetNodes();
        return WaveAlgorithmForAOEWeapon(userCell, radius, ignoreBusyCell, diagonalStep);
    }

    private List<Cell> AStar(Cell userCell, Cell targetCell)
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
                if(!item.Busy && item.weight < smallestWeightNode.weight)
                    smallestWeightNode = item;
            }

            foreach (var item in smallestWeightNode.neighbors)
            {

                if (item.coordinates == new Vector2Int(targetCell.coordinates.x, targetCell.coordinates.y))
                {
                    item.previous = smallestWeightNode;
                    List<Cell> path = new List<Cell>();
                    var tempBackTrackNode = item;

                    Cell[,] currentLayer = null;
                    switch (userCell.mapLayer)
                    {
                        case MapLayer.Projectile: currentLayer = map.projectilesLayer; break;
                        case MapLayer.DefaultUnit: currentLayer = map.unitsLayer; break;
                        case MapLayer.Surface: currentLayer = map.surfacesLayer; break;
                    }

                    while (tempBackTrackNode.coordinates != new Vector2Int(userCell.coordinates.x, userCell.coordinates.y))
                    {
                        path.Insert(0, currentLayer[tempBackTrackNode.coordinates.x, tempBackTrackNode.coordinates.y]);
                        tempBackTrackNode = tempBackTrackNode.previous;
                    }

                    return path;
                }
                else if (!item.usedToPathFinding && !item.Busy)
                {
                    _currentNodes.Add(item);
                    item.weight = Vector2Int.Distance(item.coordinates, userCell.coordinates) + Vector2Int.Distance(item.coordinates, targetCell.coordinates);
                    item.previous = smallestWeightNode;
                    item.usedToPathFinding = true;
                }
            }

            _currentNodes.Remove(smallestWeightNode);
            smallestWeightNode.usedToPathFinding = true;
        }

        return null;
    }

    private List<Cell> WaveAlgorithm(Cell userCell, int actionPoints)
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
                    if (!child.usedToPathFinding && !child.Busy)
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
    }

    private void InitializeNodesGrid()
    {
        _nodesGrid = new PathFinderNode[map.sizeX, map.sizeY];

        for (int i = 0; i < map.sizeX; i++)
        {
            for (int j = 0; j < map.sizeY; j++)
            {
                _nodesGrid[i,j] = new PathFinderNode(map.unitsLayer[i, j]);
            }
        }

    }

    private void FindAllNodesNeighbors() //todo спросить про альтернативы try catch
    {
        foreach (var node in _nodesGrid)
        {
            foreach (var dir in _dirVectors)
            {
                try
                {
                    node.AddNeighbor(_nodesGrid[node.cell.coordinates.x + dir.x, node.cell.coordinates.y + dir.y]);
                }
                catch
                {
                    // раз уж на то пошло то у меня есть конкретный вопрос:
                    // я знаю, что это несколько грубое решение проблемы, но прописывать кучу if(coordinates < 0) и прочее тоже как-то не особо изящно
                    // собственно вопрос, есть ли альтернативы? Просто лишних 4 ифа не очень как-то прописывать, когда условие то по сути одно "если вышел за пределы массива"
                }
            }
        }
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

    public bool Busy => !cell.IsEmpty;

    public bool IsWall => cell.Content as Wall;

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
}
