using FroguesFramework;
using System.Collections.Generic;
using UnityEngine;

public class SmallestAngleFinder : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private List<Transform> vectorObjects;
    [SerializeField] private Transform target;
    [SerializeField] private Transform indicator;

    void Update()
    {
        Transform smallestAngleTransform = vectorObjects[0];
        Vector3 centerScreenPoint = Camera.main.WorldToScreenPoint(transform.position).ToVector2();

        for (int i = 1; i < vectorObjects.Count; i++)
        {
            Vector3 vectorObjectScreenPoint = Camera.main.WorldToScreenPoint(vectorObjects[i].position).ToVector2();

            if (Vector3.Angle(vectorObjectScreenPoint - centerScreenPoint, Input.mousePosition - centerScreenPoint) <
                Vector3.Angle(Camera.main.WorldToScreenPoint(smallestAngleTransform.position) - centerScreenPoint, Input.mousePosition - centerScreenPoint))
            {
                smallestAngleTransform = vectorObjects[i];
            }
        }

        indicator.position = smallestAngleTransform.position;
    }
}
