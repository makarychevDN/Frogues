using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbleToSkipTurn : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        public UnityEvent OnSkipTurn;

        public void AutoSkip()
        {
            if (!UnitsQueue.Instance.IsUnitCurrent(unit))
                return;

            /*if (unit.input as PlayerInput)
            {
                var input = unit.input as PlayerInput;
                input.InputIsPossible = false;
            }*/

            OnSkipTurn.Invoke();
            UnitsQueue.Instance.ActivateNext();
        }
    }
}