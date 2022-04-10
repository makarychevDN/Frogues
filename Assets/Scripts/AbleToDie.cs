using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbleToDie : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        public UnityEvent OnDeath;

        public void Die()
        {
            if (unit.currentCell != null)
                unit.currentCell.Content = null;
            UnitsQueue.Instance.Remove(unit);
            Destroy(unit.gameObject);
            OnDeath.Invoke();
        }
    }
}