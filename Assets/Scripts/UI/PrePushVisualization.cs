using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePushVisualization : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private Vector2IntContainer prePushValueContainer;

    void Update()
    {
        arrow.localPosition = prePushValueContainer.Content.ToVector3();
    }
}
