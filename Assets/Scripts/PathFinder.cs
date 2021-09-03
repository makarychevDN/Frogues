using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance;
    public MapBasedOnTilemap map;
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

    public List<Cell> GetCellsAreaByActionPoints(Cell userCell, int actionPoints)
    {
        ResetNodes();
        return WaveAlgorithm(userCell, actionPoints);
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
