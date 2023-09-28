using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CollidersContainer : MonoBehaviour
    {
        private List<Collider> colliders = new List<Collider>();
        private List<Cell> cells = new List<Cell>();
        public List<Cell> Cells => cells;

        private void OnTriggerEnter(Collider other)
        {
            colliders.Add(other);

            var cell = other.transform.GetComponentInParent<Cell>();
            if (cell == null)
                return;

            cells.Add(cell);
        }

        private void OnTriggerExit(Collider other)
        {
            colliders.Remove(other);

            var cell = other.transform.GetComponentInParent<Cell>();
            if (cell == null)
                return;

            cells.Remove(cell);
        }

        private void OnDisable()
        {
            colliders.Clear();
            cells.Clear();
        }
    }
}