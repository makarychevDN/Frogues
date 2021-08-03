using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType _unitType;
    public Movable _movable;
    public Cell currentCell;
}

public enum UnitType
{
    DefaultUnit = 1, Projectile = 2, Surface = 3
}
