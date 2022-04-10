using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class EventOnStart : MonoBehaviour
    {
        public UnityEvent OnStart;

        void Start()
        {
            OnStart.Invoke();
        }
    }
}