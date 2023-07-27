using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CollidersContainer : MonoBehaviour
    {
       private List<Collider> colliders = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            colliders.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            colliders.Remove(other);
        }

        private void OnDisable()
        {
            colliders.Clear();
        }

        private void Update()
        {
            print(colliders.Count);
        }
    }
}