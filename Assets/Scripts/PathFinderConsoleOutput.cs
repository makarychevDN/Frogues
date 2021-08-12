using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderConsoleOutput : MonoBehaviour
{
    [SerializeField] private PathFinder _pathfinder;
    [SerializeField] private Unit _player;
    [SerializeField] private Unit _enemie;

    public void Print()
    {
        _pathfinder.FindWay(_enemie._currentCell, _player._currentCell).ForEach(cell => print(cell._coordinates));
    }
}
