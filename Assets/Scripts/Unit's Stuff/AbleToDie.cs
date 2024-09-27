using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbleToDie : MonoBehaviour
    {
        public UnityEvent OnDeath;
        public UnityEvent OnOwnerWasKilled;
        public UnityEvent OnOwnerKilledHimself; 
        private Unit _unit;
        public void Init(Unit unit)
        {
            _unit = unit;
        }
        
        public void Die(bool ownerKilledItSelf = false)
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Add(this);
            _unit.Animator.SetTrigger(CharacterAnimatorParameters.Death);
            StartCoroutine(nameof(DelayBeforeDeath), ownerKilledItSelf);
        }

        public void DieWithoutAnimation() => RemoveUnitFromTheGame();

        private void RemoveUnitFromTheGame(bool ownerKilledItSelf = false)
        {
            if (_unit.CurrentCell != null)
            {
                if(_unit.CurrentCell.Content == _unit)
                    _unit.CurrentCell.Content = null;

                if (_unit.CurrentCell.Surfaces.Contains(_unit))
                    _unit.CurrentCell.Surfaces.Remove(_unit);
            }

            _unit.CurrentRoom.CurrentlyActiveObjects.Remove(this);
            _unit.CurrentRoom.UnitsQueue.Remove(_unit);
            _unit.CurrentRoom.InvokeOnSomeoneDied();
            OnDeath.Invoke();
            Destroy(_unit.gameObject);

            if (ownerKilledItSelf)
            {
                OnOwnerKilledHimself.Invoke();
            }
            else
            {
                OnOwnerWasKilled.Invoke();
            }
        }

        private IEnumerator DelayBeforeDeath(bool ownerKilledItSelf)
        {
            yield return new WaitForSeconds(0.75f);
            RemoveUnitFromTheGame(ownerKilledItSelf);
        }
    }
}