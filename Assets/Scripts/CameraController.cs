using UnityEngine;

namespace FroguesFramework
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float distanceFromCameraToCenter = 5f;
        [SerializeField] private Transform cameraRotationPoint;
        [SerializeField] private float speed = 5;
        
        public void Init()
        {
            Camera.main.transform.parent = cameraRotationPoint;
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localPosition -= Camera.main.transform.forward * distanceFromCameraToCenter;
        }
        
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                transform.Rotate(transform.up, Input.GetAxis("Mouse X"));
            }

            Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }

        public void Deactivate() => Camera.main.transform.parent = null;

        public void Move(Vector2 movement)
        {
            cameraRotationPoint.transform.localPosition += new Vector3(movement.x, 0, movement.y) * Time.deltaTime * speed;
        }
    }
}