using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MapLayer _unitType;
    public Movable _movable;
    public Cell currentCell;
}

public enum MapLayer
{
    Projectile = 0, DefaultUnit = 1, Surface = 2
}
