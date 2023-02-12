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
            CurrentlyActiveObjects.Add(this);
            if (_unit.CurrentCell != null)
                _unit.CurrentCell.Content = null;
            
            _unit.Animator.SetTrigger(CharacterAnimatorParameters.Death);
            
            Invoke(nameof(RemoveUnitFromTheGame), 0.75f);
        }

        private void RemoveUnitFromTheGame()
        {
            CurrentlyActiveObjects.Remove(this);
            Room.Instance.UnitsQueue.Remove(_unit);
            Destroy(_unit.gameObject);
            OnDeath.Invoke();
        }
    }
}