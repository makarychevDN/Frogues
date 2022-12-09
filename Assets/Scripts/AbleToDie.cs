using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbleToDie : MonoBehaviour
    {
        public UnityEvent OnDeath;
        private Unit _unit;

        public void Init(Unit unit)
        {
            _unit = unit;
        }
        
        public void Die()
        {
            if (_unit.CurrentCell != null)
                _unit.CurrentCell.Content = null;
            UnitsQueue.Instance.Remove(_unit);
            Destroy(_unit.gameObject);
            OnDeath.Invoke();
        }
    }
}