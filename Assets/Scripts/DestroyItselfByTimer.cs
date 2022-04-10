using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class DestroyItselfByTimer : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy;
        public UnityEvent OnSpawn;
        private float _timer;

        void Start()
        {
            OnSpawn.Invoke();
        }

        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= timeToDestroy)
                Destroy(gameObject);
        }
    }
}