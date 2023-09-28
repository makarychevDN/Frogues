using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class NearestHexDirectionToMouseFinder : MonoBehaviour
    {
        [SerializeField] private List<Transform> vectorObjects;

        public Transform FindNearestVectorToCursor(Vector3 cursorPosition)
        {
            Transform smallestAngleTransform = vectorObjects[0];
            Vector3 centerScreenPoint = Camera.main.WorldToScreenPoint(transform.position).ToVector2();

            for (int i = 1; i < vectorObjects.Count; i++)
            {
                Vector3 vectorObjectScreenPoint = Camera.main.WorldToScreenPoint(vectorObjects[i].position).ToVector2();

                if (Vector3.Angle(vectorObjectScreenPoint - centerScreenPoint, cursorPosition - centerScreenPoint) <
                    Vector3.Angle(Camera.main.WorldToScreenPoint(smallestAngleTransform.position) - centerScreenPoint, cursorPosition - centerScreenPoint))
                {
                    smallestAngleTransform = vectorObjects[i];
                }
            }

            return smallestAngleTransform;
        }
    }
}