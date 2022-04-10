using UnityEngine.Events;

namespace FroguesFramework
{
    public class EventOnInput : BaseInput
    {
        public UnityEvent OnInput;

        public override void Act()
        {
            OnInput.Invoke();
            OnInputDone.Invoke();
        }
    }
}
