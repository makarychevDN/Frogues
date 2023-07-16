using UnityEngine;

namespace FroguesFramework
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float distanceFromCameraToCenter = 5f;
        [SerializeField] private Transform cameraRotationPoint;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float zoomingSpeed = 5f;
        private Transform _camera;
        
        public void Init()
        {
            _camera = Camera.main.transform;
            _camera.parent = cameraRotationPoint;
            _camera.localPosition = Vector3.zero;
            _camera.localRotation = Quaternion.Euler(30f, 0, 0);
            _camera.localPosition -= Camera.main.transform.forward * distanceFromCameraToCenter;
        }
        
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                cameraRotationPoint.Rotate(cameraRotationPoint.up, Input.GetAxis("Mouse X"));
            }

            Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }

        public void Deactivate() => Camera.main.transform.parent = null;

        public void Move(Vector2 movement)
        {
            cameraRotationPoint.transform.localPosition += cameraRotationPoint.TransformDirection(new Vector3(movement.x, 0, movement.y)) * Time.deltaTime * movementSpeed;
        }
        
        public void Zoom(float value)
        {
            _camera.transform.position += _camera.forward * value * zoomingSpeed;
        }
    }
}