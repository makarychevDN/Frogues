using UnityEngine;

namespace FroguesFramework
{
    public class CameraRotationPointController : MonoBehaviour
    {
        [SerializeField] private float distanceFromCameraToCenter = 5f;
        
        public void Init()
        {
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localPosition -= Camera.main.transform.forward * distanceFromCameraToCenter;
        }
        
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                transform.Rotate(transform.up, Input.GetAxis("Mouse X"));
            }
        }

        public void Deactivate() => Camera.main.transform.parent = null;
    }
}