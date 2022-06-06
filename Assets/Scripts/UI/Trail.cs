using UnityEngine;

namespace FroguesFramework
{
    public class Trail : MonoBehaviour
    {
        [SerializeField] private GameObject sprite;
        [SerializeField] private Vector2 trailDirection;
        public Vector2 TrailDirection => trailDirection.normalized;

        public void Enable(bool value)
        {
            sprite.SetActive(value);
        }
    }
}