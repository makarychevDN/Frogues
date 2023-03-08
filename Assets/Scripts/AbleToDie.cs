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
            _unit.Animator.SetTrigger(CharacterAnimatorParameters.Death);
            Invoke(nameof(RemoveUnitFromTheGame), 0.75f);
        }

        public void DieWithoutAnimation() => RemoveUnitFromTheGame();

        private void RemoveUnitFromTheGame()
        {
            if (_unit.CurrentCell != null)
                _unit.CurrentCell.Content = null;
            
            CurrentlyActiveObjects.Remove(this);
            EntryPoint.Instance.UnitsQueue.Remove(_unit);
            Destroy(_unit.gameObject);
            OnDeath.Invoke();
        }
    }
}