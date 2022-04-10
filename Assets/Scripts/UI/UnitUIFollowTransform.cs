using UnityEngine;

namespace FroguesFramework
{
    public class UnitUIFollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.position = target.transform.position;
        }
    }
}