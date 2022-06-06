using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TrailsEnabler : MonoBehaviour
    {
        [SerializeField] private List<Trail> trails;
        private const float TOLERANCE = 0.01f;

        public void EnableTrail(Vector2 direction)
        {
            trails.FirstOrDefault(trail =>
                Math.Abs(trail.TrailDirection.x - direction.x) < TOLERANCE &&
                Math.Abs(trail.TrailDirection.y - direction.y) < TOLERANCE)?.Enable(true);
        }

        public void DisableTrails()
        {
            trails.ForEach(trail => trail.Enable(false));
        }
    }
}