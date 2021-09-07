using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    public static Vector3 ToVector3(this Vector2Int vector2Int)
    {
        return new Vector3(vector2Int.x, vector2Int.y, 0);
    }
    
    public static Vector2Int ToVector2Int(this Vector3 vector3)
    {
        return new Vector2Int((int)vector3.x, (int)vector3.y);
    }
}
