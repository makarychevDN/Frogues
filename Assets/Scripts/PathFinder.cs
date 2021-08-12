using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public MapBasedOnTilemap _map;
    public PathFinderNode[,] _nodesGrid;
    private List<Vector2Int> _dirVectors;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _linesParent;

    private void Start()
    {
        InitializeDirVectors();
        InitializeNodesGrid();
        FindAllNodesNeighbors();
    }

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
                _nodesGrid[i,j] = new PathFinderNode(_map._surfacesLayer[i, j]);
                if(!_map._surfacesLayer[i, j].isEmpty)
                {
                    _nodesGrid[i, j]._busy = false;
                }
            }
        }

    }

    private void FindAllNodesNeighbors()
    {
        foreach (var node in _nodesGrid)
        {
            foreach (var dir in _dirVectors)
            {
                try
                {
                    node.AddNeighbor(_nodesGrid[node._cell._coordinates.x + dir.x, node._cell._coordinates.y + dir.y]);
                    var line = Instantiate(_lineRenderer);
                    line.SetPosition(0, node._cell.transform.position);
                    line.SetPosition(1, _nodesGrid[node._cell._coordinates.x + dir.x, node._cell._coordinates.y + dir.y]._cell.transform.position);
                    line.transform.parent = _linesParent;
                }
                catch
                {

                }
            }
        }
    }
}

public class PathFinderNode
{
    public Cell _cell;
    public List<PathFinderNode> _neighbors;
    public bool _usedToPathFinding;
    public bool _busy;
    public PathFinderNode _previous;
    public float _weight;

    public PathFinderNode(Cell cell)
    {
        _cell = cell;
        _neighbors = new List<PathFinderNode>();
    }

    public void AddNeighbor(PathFinderNode neighbor)
    {
        _neighbors.Add(neighbor);
    }
}
