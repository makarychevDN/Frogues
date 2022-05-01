using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public static class Extensions
    {
        public static Vector3 ToVector3(this Vector2Int vector2Int)
        {
            return new Vector3(vector2Int.x, vector2Int.y, 0);
        }

        public static Vector2Int ToVector2Int(this Vector3 vector3)
        {
            return new Vector2Int((int) vector3.x, (int) vector3.y);
        }

        public static Vector2Int ToVector2Int(this Vector3Int vector3)
        {
            return new Vector2Int(vector3.x, vector3.y);
        }

        public static List<Cell> WithContentOnly(this List<Cell> cells)
        {
            return cells.Where(cell => cell.Content != null).ToList();
        }

        public static T GetFirst<T>(this List<T> list) => list[0];

        public static T GetLast<T>(this List<T> list) => list[list.Count - 1];

        public static void SwitchActive(this GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}