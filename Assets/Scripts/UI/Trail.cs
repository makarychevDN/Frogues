using UnityEngine;

namespace FroguesFramework
{
    public class Trail : MonoBehaviour
    {
        [SerializeField] private GameObject sprite;
        [SerializeField] private Vector2Int trailDirection;
        public Vector2Int TrailDirection => trailDirection;

        public void Enable(bool value)
        {
            sprite.SetActive(value);
        }
    }
}