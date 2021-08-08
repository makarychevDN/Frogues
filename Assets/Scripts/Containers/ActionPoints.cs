using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoints : MonoBehaviour
{
    public IntContainer _currentPoints;
    public IntContainer _maxPointsCount;
    public IntContainer _pointsRegeneration;

    public void RegeneratePoints()
    {
        _currentPoints.Content += _pointsRegeneration.Content;
        _currentPoints.Content = Mathf.Clamp(_currentPoints.Content, 0, _maxPointsCount.Content);
    }
}
