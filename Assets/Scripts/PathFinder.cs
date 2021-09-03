using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance;
    public MapBasedOnTilemap _map;
    public PathFinderNode[,] _nodesGrid;
    private List<Vector2Int> _dirVectors;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _linesParent;
    private List<PathFinderNode> _currentNodes;
    private List<PathFinderNode> _childNodes;

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
            item._usedToPathFinding = false;
            item._weight = 0;
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
        _nodesGrid[userCell.coordinates.x, userCell.coordinates.y]._usedToPathFinding = true;
        PathFinderNode smallestWeightNode;

        while (_currentNodes.Count != 0)
        {
            smallestWeightNode = _currentNodes[0];

            foreach (var item in _currentNodes)
            {
                if(!item.Busy && item._weight < smallestWeightNode._weight)
                    smallestWeightNode = item;
            }

            foreach (var item in smallestWeightNode._neighbors)
            {

                if (item._coordinates == new Vector2Int(targetCell.coordinates.x, targetCell.coordinates.y))
                {
                    item._previous = smallestWeightNode;
                    List<Cell> path = new List<Cell>();
                    var tempBackTrackNode = item;

                    Cell[,] currentLayer = null;
                    switch (userCell.mapLayer)
                    {
                        case MapLayer.Projectile: currentLayer = _map._projectilesLayer; break;
                        case MapLayer.DefaultUnit: currentLayer = _map._unitsLayer; break;
                        case MapLayer.Surface: currentLayer = _map._surfacesLayer; break;
                    }

                    while (tempBackTrackNode._coordinates != new Vector2Int(userCell.coordinates.x, userCell.coordinates.y))
                    {
                        path.Insert(0, currentLayer[tempBackTrackNode._coordinates.x, tempBackTrackNode._coordinates.y]);
                        tempBackTrackNode = tempBackTrackNode._previous;
                    }

                    return path;
                }
                else if (!item._usedToPathFinding && !item.Busy)
                {
                    _currentNodes.Add(item);
                    item._weight = Vector2Int.Distance(item._coordinates, userCell.coordinates) + Vector2Int.Distance(item._coordinates, targetCell.coordinates);
                    item._previous = smallestWeightNode;
                    item._usedToPathFinding = true;
                }
            }

            _currentNodes.Remove(smallestWeightNode);
            smallestWeightNode._usedToPathFinding = true;
        }

        return null;
    }

    private List<Cell> WaveAlgorithm(Cell userCell, int actionPoints)
    {
        _childNodes = new List<PathFinderNode>();
        _currentNodes = new List<PathFinderNode>();
        _currentNodes.Add(_nodesGrid[userCell.coordinates.x, userCell.coordinates.y]);
        _nodesGrid[userCell.coordinates.x, userCell.coordinates.y]._usedToPathFinding = true;
        int stepCounter = 0;
        List<Cell> resultCells = new List<Cell>();

        while (stepCounter < actionPoints || _childNodes.Count != 0)
        {
            foreach (var parent in _currentNodes)
            {
                foreach (var child in parent._neighbors)
                {
                    if (!child._usedToPathFinding && !child.Busy)
                    {
                        child._previous = parent;
                        _childNodes.Add(child);
                        child._usedToPathFinding = true;
                        resultCells.Add(child._cell);
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
        _nodesGrid = new PathFinderNode[_map._sizeX, _map._sizeY];

        for (int i = 0; i < _map._sizeX; i++)
        {
            for (int j = 0; j < _map._sizeY; j++)
            {
                _nodesGrid[i,j] = new PathFinderNode(_map._unitsLayer[i, j]);
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
                    node.AddNeighbor(_nodesGrid[node._cell.coordinates.x + dir.x, node._cell.coordinates.y + dir.y]);
                    var line = Instantiate(_lineRenderer);
                    line.SetPosition(0, node._cell.transform.position);
                    line.SetPosition(1, _nodesGrid[node._cell.coordinates.x + dir.x, node._cell.coordinates.y + dir.y]._cell.transform.position);
                    line.transform.parent = _linesParent;
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
    public Cell _cell;
    public Vector2Int _coordinates;
    public List<PathFinderNode> _neighbors;
    public bool _usedToPathFinding;
    public PathFinderNode _previous;
    public float _weight;

    public bool Busy => !_cell.IsEmpty;

    public PathFinderNode(Cell cell)
    {
        _cell = cell;
        _neighbors = new List<PathFinderNode>();
        _coordinates = _cell.coordinates;
    }

    public void AddNeighbor(PathFinderNode neighbor)
    {
        _neighbors.Add(neighbor);
    }
}
