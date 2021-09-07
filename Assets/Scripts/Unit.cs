using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MapLayer unitType;
    public Movable movable;
    public Cell currentCell;
    public BaseInput input;
    public Pusher pusher;
    public Pushable pushable;

    public Vector2Int Coordinates => currentCell.coordinates;
}

