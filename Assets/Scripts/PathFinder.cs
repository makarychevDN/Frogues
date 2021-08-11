using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private MapBasedOnTilemap map;
    private PathFinderNode[,] nodesGrid;
    List<PathFinderNode> currentNodes;
    List<PathFinderNode> childNodes;
    private List<Vector2Int> dirVectors;

    public PathFinderNode[,] NodesGrid { get => nodesGrid; set => nodesGrid = value; }

    public PathFinder(MapBasedOnTilemap map)
    {
        this.map = map;
        InitializeDirVectors();
        InitialaizeNodesGrid(map._sizeX, map._sizeY);
        FindAllNodesNeighbors(map._sizeX, map._sizeY);
    }
    public void ResetNodes()
    {
        foreach (var item in nodesGrid)
        {
            item.UsedToPathFinding = false;
            item.Weight = 0;
        }
    }
    public List<Cell> FindWay(Unit user, Unit target, bool ignoreTraps)
    {
        ResetNodes();
        return AStar(user, target, ignoreTraps);
    }

    #region pathfinding algorithms
    /*private List<Cell> WaveAlgorithm(MapObject user, MapObject target, bool ignoreTraps)
    {
        childNodes = new List<PathFinderNode>();
        childNodes.Add(nodesGrid[user.Pos.x, user.Pos.y]);
        nodesGrid[user.Pos.x, user.Pos.y].UsedToPathFinding = true;

        while (childNodes.Count != 0)
        {
            currentNodes = childNodes;
            childNodes = new List<PathFinderNode>();

            foreach (var tempParent in currentNodes)
            {
                foreach (var tempChild in tempParent.Neighbors)
                {
                    if (tempChild.Pos == new Vector2Int(target.Pos.x, target.Pos.y))
                    {
                        tempChild.Previous = tempParent;
                        List<Vector2Int> path = new List<Vector2Int>();
                        var tempBackTrackNode = tempChild;

                        while (tempBackTrackNode.Pos != new Vector2Int(user.Pos.x, user.Pos.y))
                        {
                            path.Insert(0, tempBackTrackNode.Pos);
                            tempBackTrackNode = tempBackTrackNode.Previous;
                        }

                        return path;
                    }
                    else if (!tempChild.UsedToPathFinding && !tempChild.Busy && (map.GetSurfaceByVector(tempChild.Pos) == null || ignoreTraps))
                    {
                        tempChild.Previous = tempParent;
                        childNodes.Add(tempChild);
                        tempChild.UsedToPathFinding = true;
                    }
                }
            }
        }
        return null;
    }*/

    private List<Cell> AStar(Unit user, Unit target, bool ignoreTraps)
    {
        currentNodes = new List<PathFinderNode>();
        currentNodes.Add(nodesGrid[user._currentCell._coordinates.x, user._currentCell._coordinates.y]);
        nodesGrid[user._currentCell._coordinates.x, user._currentCell._coordinates.y].UsedToPathFinding = true;
        PathFinderNode smallestWeightNode;

        int userXPos = user._currentCell._coordinates.x;
        int userYPos = user._currentCell._coordinates.y;

        while (currentNodes.Count != 0)
        {
            smallestWeightNode = currentNodes[0];

            foreach (var item in currentNodes)
            {
                if (item.Weight < smallestWeightNode.Weight)
                    smallestWeightNode = item;
            }

            foreach (var item in smallestWeightNode.Neighbors)
            {
                if (item.Pos == new Vector2Int(userXPos, userYPos))
                {
                    item.Previous = smallestWeightNode;
                    List<Cell> path = new List<Cell>();
                    var tempBackTrackNode = item;

                    while (tempBackTrackNode.Pos != new Vector2Int(userXPos, userYPos))
                    {
                        path.Insert(0, tempBackTrackNode.Cell);
                        tempBackTrackNode = tempBackTrackNode.Previous;
                    }

                    return path;
                }
                else if (!item.UsedToPathFinding && !item.Busy /*&& (map.GetSurfaceByVector(item.Pos) == null || ignoreTraps)*/)
                {
                    currentNodes.Add(item);
                    item.Weight = Vector2Int.Distance(item.Pos, user._currentCell._coordinates) + Vector2Int.Distance(item.Pos, target._currentCell._coordinates);
                    item.Previous = smallestWeightNode;
                    item.UsedToPathFinding = true;
                }
            }

            currentNodes.Remove(smallestWeightNode);
            smallestWeightNode.UsedToPathFinding = true;
        }

        return null;
    }
    #endregion

    #region InitPathfinder
    private void InitializeDirVectors()
    {
        dirVectors = new List<Vector2Int>();
        dirVectors.Add(Vector2Int.up);
        dirVectors.Add(Vector2Int.right);
        dirVectors.Add(Vector2Int.down);
        dirVectors.Add(Vector2Int.left);
    }

    private void InitialaizeNodesGrid(int sizeX, int sizeY)
    {
        nodesGrid = new PathFinderNode[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                nodesGrid[i, j] = new PathFinderNode(i, j, map._unitsLayer[i, j]);

                if (!map._unitsLayer[i, j].isEmpty)
                {
                    nodesGrid[i, j].Busy = true;
                }
            }
        }
    }

    private void FindAllNodesNeighbors(int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (nodesGrid[i, j] != null)
                {
                    foreach (var temp in dirVectors)
                    {
                        if (i + temp.x >= 0 && i + temp.x < sizeX && j + temp.y >= 0 && j + temp.y < sizeY)
                        {
                            if (nodesGrid[i + temp.x, j + temp.y] != null)
                            {
                                nodesGrid[i, j].AddNeighbor(nodesGrid[i + temp.x, j + temp.y]);
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

}

public class PathFinderNode
{
    private Cell cell;
    private Vector2Int pos;
    private List<PathFinderNode> neighbors;
    private bool usedToPathFinding;
    private bool busy;
    private PathFinderNode previous;
    private float weight;

    public PathFinderNode(int x, int y, Cell cell)
    {
        this.cell = cell;
        pos = new Vector2Int(x, y);
        neighbors = new List<PathFinderNode>();
    }

    public void AddNeighbor(PathFinderNode neighbor)
    {
        neighbors.Add(neighbor);
    }

    #region properties
    public List<PathFinderNode> Neighbors { get => neighbors; set => neighbors = value; }
    public bool UsedToPathFinding { get => usedToPathFinding; set => usedToPathFinding = value; }
    public Vector2Int Pos { get => pos; set => pos = value; }
    public PathFinderNode Previous { get => previous; set => previous = value; }
    public bool Busy { get => busy; set => busy = value; }
    public float Weight { get => weight; set => weight = value; }
    public Cell Cell { get => cell; set => cell = value; }

    #endregion
}