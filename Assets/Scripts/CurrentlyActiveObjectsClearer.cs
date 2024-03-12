using UnityEngine;

namespace FroguesFramework
{
    public class CurrentlyActiveObjectsClearer : MonoBehaviour
    {
        private void Start()
        {
            CurrentlyActiveObjects.Clear();
        }
    }
}