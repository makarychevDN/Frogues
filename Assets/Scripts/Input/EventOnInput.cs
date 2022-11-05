using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class EventOnInput : MonoBehaviour, IAbleToAct
    {
        public UnityEvent OnInput;

        public void Act()
        {
            OnInput.Invoke();
        }

        public void Init()
        {
            
        }
    }
}
