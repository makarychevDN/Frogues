using System.Linq;
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
        private float maxXPosition, minXPosition, maxZPosition, minZPosition;

        public void Init()
        {
            _camera = Camera.main.transform;
            _camera.parent = cameraRotationPoint;
            _camera.localPosition = Vector3.zero;
            _camera.localRotation = Quaternion.Euler(30f, 0, 0);
            _camera.localPosition -= Camera.main.transform.forward * distanceFromCameraToCenter;

            maxXPosition = EntryPoint.Instance.Map.allCells.Max(cell => cell.transform.position.x);
            minXPosition = EntryPoint.Instance.Map.allCells.Min(cell => cell.transform.position.x);
            maxZPosition = EntryPoint.Instance.Map.allCells.Max(cell => cell.transform.position.z);
            minZPosition = EntryPoint.Instance.Map.allCells.Min(cell => cell.transform.position.z);
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
            var clampedXPosition = Mathf.Clamp(cameraRotationPoint.transform.position.x, minXPosition, maxXPosition);
            var clampedZPosition = Mathf.Clamp(cameraRotationPoint.transform.position.z, minZPosition, maxZPosition);
            cameraRotationPoint.transform.position = new Vector3(clampedXPosition, 0, clampedZPosition);
        }
        
        public void Zoom(float value)
        {
            Vector3 updatedPosition = _camera.transform.position + _camera.forward * value * zoomingSpeed;
            Vector3 clampedPosition = Extensions.ClampMagnitude(_camera.forward * (cameraRotationPoint.position - updatedPosition).magnitude, 10, 3);
            _camera.transform.position = cameraRotationPoint.position - clampedPosition;
        }
    }
}