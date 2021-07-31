using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    public static Vector3 ToVector3(this Vector2Int vector2Int)
    {
        return new Vector3(vector2Int.x, vector2Int.y, 0);
    }
}
