using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class EllipsisCollider : MonoBehaviour
    {
        [SerializeField] private EdgeCollider2D edgeCollider2D;
        [SerializeField, Range(2, 50)] private int smoothness;
        [SerializeField] private float xRadius, yRadius;

        private void OnValidate()
        {
            var calculatedPoints = new List<Vector2>();
            edgeCollider2D = GetComponent<EdgeCollider2D>();
            var deltaAngle = 360f / smoothness;
            var deltaRadian = deltaAngle * Mathf.Deg2Rad;

            for (int i = 0; i < smoothness; i++)
            {
                calculatedPoints.Add(new Vector2(Mathf.Cos(deltaRadian * i) * xRadius, Mathf.Sin(deltaRadian * i)) *
                                     yRadius);
            }

            calculatedPoints.Add(calculatedPoints[0]);

            edgeCollider2D.points = calculatedPoints.ToArray();
        }
    }
}