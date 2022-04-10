using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class OnKeyDownUnityEvent : MonoBehaviour
    {
        [SerializeField] private KeyCode expectedKey;
        public UnityEvent OnKeyDown;

        void Update()
        {
            if (Input.GetKeyDown(expectedKey))
                OnKeyDown.Invoke();
        }
    }
}