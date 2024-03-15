using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float distanceFromCameraToCenter = 5f;
        [SerializeField] private Transform cameraRotationPoint;
        [SerializeField] private float movementSpeed = 5f;
        private float _zoomingSpeed = 600f;
        private float _rotationSpeed = 500f;
        private Transform _camera;
        private float _maxAllowedDistanceToMoveCamera;
        public UnityEvent OnCameraReseted;
        public UnityEvent OnCameraRotated;

        public void Init()
        {
            _camera = Camera.main.transform;
            _camera.parent = cameraRotationPoint;
            ResetCamera();
            _maxAllowedDistanceToMoveCamera = EntryPoint.Instance.Map.allCells.Max(cell => (cell.transform.position - transform.position).magnitude) * 1.25f;
        }

        public void Deactivate() => Camera.main.transform.parent = null;

        public void Move(Vector2 movement)
        {
            Vector3 newPos = cameraRotationPoint.transform.position + cameraRotationPoint.TransformDirection(new Vector3(movement.x, 0, movement.y)) * Time.deltaTime * movementSpeed;
            Vector3 offset = newPos - transform.position;
            cameraRotationPoint.transform.position = transform.position + Vector3.ClampMagnitude(offset, _maxAllowedDistanceToMoveCamera);
        }

        public void Rotate(float value)
        {
            cameraRotationPoint.Rotate(cameraRotationPoint.up, value * _rotationSpeed * Time.deltaTime);
            OnCameraRotated.Invoke();
        }

        public void Zoom(float value)
        {
            Vector3 updatedPosition = _camera.transform.position + _camera.forward * value * _zoomingSpeed * Time.deltaTime;
            Vector3 clampedPosition = Extensions.ClampMagnitude(_camera.forward * (cameraRotationPoint.position - updatedPosition).magnitude, 10, 3);
            _camera.transform.position = cameraRotationPoint.position - clampedPosition;
        }

        public void ResetCamera()
        {
            cameraRotationPoint.transform.position = transform.position;
            cameraRotationPoint.transform.rotation = transform.rotation;
            _camera.localPosition = Vector3.zero;
            _camera.localRotation = Quaternion.Euler(30f, 0, 0);
            _camera.localPosition -= Camera.main.transform.forward * distanceFromCameraToCenter;
            OnCameraReseted.Invoke();
        }
    }
}