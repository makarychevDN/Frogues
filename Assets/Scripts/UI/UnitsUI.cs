using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> visualisationObjects;

    public void SetActiveForAllVisualizationObjects(bool value)
    {
        visualisationObjects.ForEach(obj => obj.SetActive(value));
    }
}
