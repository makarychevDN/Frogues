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
        
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
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

        /// <summary>
        /// return 0 on false and 1 on true
        /// </summary>
        /// <param name="value"></param>
        public static int ToInt(this bool value) => value ? 1 : 0;
        
        /// <summary>
        /// return false on 0 and 1 on any another value
        /// </summary>
        /// <param name="value"></param>
        public static bool ToBool(this int value) => value != 0;
        
        public static void AddUnique<T>( this IList<T> self, IEnumerable<T> items )
        {
            foreach(var item in items)
                if(!self.Contains(item))
                    self.Add(item);
        }
        
        public static bool CloseEnoughTo(this Vector2 comparableVector, Vector2 vectorToCompare, float tolerance)
        {
            return Mathf.Abs(comparableVector.x - vectorToCompare.x) < tolerance &&
                   Mathf.Abs(comparableVector.y - vectorToCompare.y) < tolerance;
        }

        private static float defaultCompareVectorTolerance = 0.01f;
        public static bool CloseEnoughTo(this Vector2 comparableVector, Vector2 vectorToCompare) =>
            (CloseEnoughTo(comparableVector, vectorToCompare, defaultCompareVectorTolerance));
    }
}