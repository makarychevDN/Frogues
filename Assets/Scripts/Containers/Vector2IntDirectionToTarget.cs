using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2IntDirectionToTarget : Vector2IntContainer
{
    [SerializeField] private Unit user;
    [SerializeField] private UnitContainer targetContainer;

    public override Vector2Int Content => CalculateDirection();

    private Vector2Int CalculateDirection()
    {
        return new Vector2Int(
            user.Coordinates.x < targetContainer.Content.Coordinates.x ? 1 : -1,
            user.Coordinates.y < targetContainer.Content.Coordinates.y ? 1 : -1
        );
    }
}
