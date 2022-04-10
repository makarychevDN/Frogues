using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitsUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> visualisationObjects;

        public void SetActiveForAllVisualizationObjects(bool value)
        {
            visualisationObjects.ForEach(obj => obj.SetActive(value));
        }

        private void Awake()
        {
            UnitsUIEnabler.UIOfAllUnits.Add(this);
        }

        private void OnDestroy()
        {
            UnitsUIEnabler.UIOfAllUnits.Remove(this);
        }
    }
}