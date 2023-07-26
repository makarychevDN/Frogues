using System;
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
            return new Vector2(vector3.x, vector3.z);
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
        
        public static bool Even(this int value) => value % 2 == 0;
        
        public static bool Odd(this int value) => value % 2 != 0;
        
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

        public static int DistanceToCell(this Cell from, Cell to)
        {
            return EntryPoint.Instance.PathFinder.FindWay(from, to, true, true, true).Count;
        }

        public static int ModificateWithStat(this int value, int statValue, float modificatorStep)
        {
            return (int)(value * (1 + statValue * modificatorStep));
        }

        public static void SetAnimationCurveShape(this LineRenderer lineRenderer, Vector3 lineOwnerPosition, Vector3 startPosition, Vector3 endPosition, float parabolaHeight, AnimationCurve parabolaCurve)
        {
            var stepOnCurve = 1f / lineRenderer.positionCount;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                var pos = PositionOnCurveCalculator.Calculate(startPosition, endPosition, parabolaCurve, stepOnCurve * i, parabolaHeight);
                lineRenderer.SetPosition(i, pos - lineOwnerPosition);
            }
        }

        public static Vector3 PositionRelativeToMainCamera(this Vector3 vector)
        {
            return Camera.main.transform.InverseTransformDirection(vector - Camera.main.transform.position);
        }
        
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0) return default(T);
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static HexDir GetHexDirByClockwiseRotation(this HexDir dir)
        {
            int dirValue = (int)dir;

            dirValue++;
            if (dirValue == 7)
                dirValue = 1;

            return (HexDir)dirValue;
        }

        public static HexDir GetHexDirByCounterClockwiseRotation(this HexDir dir) 
        {
            int dirValue = (int)dir;

            dirValue--;
            if (dirValue == 0)
                dirValue = 6;

            return (HexDir)dirValue;
        }

        public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * (double)max) return v.normalized * max;
            else if (sm < (double)min * (double)min) return v.normalized * min;
            return v;
        }

        public static int RoundWithGameRules(this float value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }
    }
}