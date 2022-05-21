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

            OnSkipTurn.Invoke();
            UnitsQueue.Instance.ActivateNext();
        }
    }
}