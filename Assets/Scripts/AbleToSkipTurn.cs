using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbleToSkipTurn : MonoBehaviour
    {
        private Unit _unit;
        public UnityEvent OnSkipTurn;

        public void Init(Unit unit)
        {
            _unit = unit;
        }
        
        public void AutoSkip()
        {
            if (!Room.Instance.UnitsQueue.IsUnitCurrent(_unit))
                return;

            OnSkipTurn.Invoke();
            Room.Instance.UnitsQueue.ActivateNext();
        }
    }
}