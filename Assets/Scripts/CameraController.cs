using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float distanceFromCameraToCenter = 5f;
        [SerializeField] private Transform cameraRotationAroundYAxisPoint;
        [SerializeField] private Transform cameraRotationAroundXAxisPoint;
        [SerializeField] private float movementSpeed = 5f;
        private float _zoomingSpeed = 600f;
        private float _rotationAroundYAxisSpeed = 500f;
        private float _rotationAroundXAxisSpeed = 500f;
        private Transform _camera;
        private float _maxAllowedDistanceToMoveCamera;
        public UnityEvent OnCameraReseted;
        public UnityEvent OnCameraRotated;

        public void Init()
        {
            _maxAllowedDistanceToMoveCamera = EntryPoint.Instance.Map.allCells.Max(cell => (cell.transform.position - transform.position).magnitude) * 1.25f;
            _camera = Camera.main.transform;
            _camera.parent = cameraRotationAroundXAxisPoint;
            ResetCamera();
        }

        public void Deactivate() => Camera.main.transform.parent = null;

        public void Move(Vector2 movement)
        {
            Vector3 newPos = cameraRotationAroundYAxisPoint.transform.position + cameraRotationAroundYAxisPoint.TransformDirection(new Vector3(movement.x, 0, movement.y)) * Time.deltaTime * movementSpeed;
            Vector3 offset = newPos - transform.position;
            cameraRotationAroundYAxisPoint.transform.position = transform.position + Vector3.ClampMagnitude(offset, _maxAllowedDistanceToMoveCamera);
        }

        public void RotateCameraAroundYAxis(float value)
        {
            cameraRotationAroundYAxisPoint.Rotate(cameraRotationAroundYAxisPoint.up, value * _rotationAroundYAxisSpeed * Time.deltaTime);
            OnCameraRotated.Invoke();
        }

        public void RotateCameraAroundXAxis(float value)
        {
            var currentXAngle = cameraRotationAroundXAxisPoint.localEulerAngles.x;
            currentXAngle -= value * _rotationAroundXAxisSpeed * Time.deltaTime;
            currentXAngle = Mathf.Clamp(currentXAngle, 20, 45);
            cameraRotationAroundXAxisPoint.localEulerAngles = new Vector3(currentXAngle, 0, 0);
            OnCameraRotated.Invoke();
        }

        public void Zoom(float value)
        {
            Vector3 updatedPosition = _camera.transform.position + _camera.forward * value * _zoomingSpeed * Time.deltaTime;
            Vector3 clampedPosition = Extensions.ClampMagnitude(_camera.forward * (cameraRotationAroundYAxisPoint.position - updatedPosition).magnitude, 10, 3);
            _camera.transform.position = cameraRotationAroundYAxisPoint.position - clampedPosition;
        }

        public void ResetCamera()
        {
            cameraRotationAroundYAxisPoint.transform.position = transform.position;
            cameraRotationAroundYAxisPoint.transform.rotation = transform.rotation;
            cameraRotationAroundXAxisPoint.transform.localEulerAngles = new Vector3(30, 0, 0);
            _camera.localPosition = Vector3.zero;
            _camera.localEulerAngles = Vector3.zero;
            _camera.localPosition -= Vector3.forward * distanceFromCameraToCenter;
            OnCameraReseted.Invoke();
        }
    }
}