using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class CurrentlyActiveObjectsDebug : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> debugCurrenlyActiveObjects;
        [SerializeField] private List<string> debugAllActivatedForSessionObjects;

        void Update()
        {
            debugCurrenlyActiveObjects = CurrentlyActiveObjects.activeObjects.ToList();
            debugAllActivatedForSessionObjects = CurrentlyActiveObjects.AllActivatedForSessionObjects.ToList();

            if (debugCurrenlyActiveObjects.Any(obj => obj == null))
                Debug.LogError("Missing Currently Active Behavipour");
        }
    }
}