using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitsUIEnabler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> uiObjects;

        private void Update()
        {
            uiObjects.ForEach(uiObject => uiObject.SetActive(!CurrentlyActiveObjects.SomethingIsActNow));
        }
    }
}