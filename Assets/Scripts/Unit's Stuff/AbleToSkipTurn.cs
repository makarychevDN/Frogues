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
            if (!_unit.CurrentRoom.UnitsQueue.IsUnitCurrent(_unit))
                return;

            OnSkipTurn.Invoke();
            _unit.CurrentRoom.UnitsQueue.ActivateNext();
            _unit.CurrentRoom.DisableAllPrevisualization();
        }
    }
}