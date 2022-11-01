using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbleToDie : MonoBehaviour
    {
        public UnityEvent OnDeath;
        private Unit _unit;

        public Unit Unit
        {
            set => _unit = value;
        }

        public void Die()
        {
            if (_unit.currentCell != null)
                _unit.currentCell.Content = null;
            UnitsQueue.Instance.Remove(_unit);
            Destroy(_unit.gameObject);
            OnDeath.Invoke();
        }
    }
}