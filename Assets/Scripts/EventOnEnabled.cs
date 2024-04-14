using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class EventOnEnabled : MonoBehaviour
    {
        public UnityEvent onEnabled;

        private void OnEnable()
        {
            onEnabled.Invoke();
        }
    }
}