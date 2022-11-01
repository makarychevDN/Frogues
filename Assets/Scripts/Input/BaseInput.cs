using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public abstract class BaseInput : MonoBehaviour
    {
        //[SerializeField] protected Unit unit;
        public UnityEvent OnInputDone;

        public abstract void Act();
    }
}