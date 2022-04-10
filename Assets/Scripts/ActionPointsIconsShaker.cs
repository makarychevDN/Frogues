using UnityEngine;

namespace FroguesFramework
{
    public class ActionPointsIconsShaker : MonoBehaviour
    {
        [SerializeField] private AnimationCurve shakingCurve;
        [SerializeField] private float shakingTime;
        [SerializeField] private float shakingAmplitude;
        private bool _shaking;
        private Vector3 _startPosition;
        private float _timer;

        private void Start()
        {
            _startPosition = transform.localPosition;
        }

        public void Shake()
        {
            _shaking = true;
            _timer = 0;
        }

        private void Update()
        {
            if (!_shaking)
                return;

            transform.localPosition =
                new Vector3(
                    _startPosition.x + shakingCurve.Evaluate(Mathf.Lerp(0, 1, _timer / shakingTime)) * shakingAmplitude,
                    _startPosition.y, _startPosition.z);
            _timer += Time.deltaTime;

            if (_timer < shakingTime)
                return;

            transform.localPosition = _startPosition;
            _shaking = false;
        }
    }
}
