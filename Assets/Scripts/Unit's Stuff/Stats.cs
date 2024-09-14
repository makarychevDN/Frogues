using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Stats : MonoBehaviour, IAbleToCalculateHashFunctionOfPrevisualisation, IRoundTickable
    {
        public UnityEvent OnSomethingUpdated;
        private Unit _owner;

        public UnityEvent OnBoostUpdated;

        [SerializeField] private int boost;

        public int Boost => boost;

        public int CalculateHashFunctionOfPrevisualisation() => 1;

        public void RemoveAllNonConstantlyEffects()
        {

        }

        public void AddBoost(int additionalValue)
        {
            boost += additionalValue;
            OnBoostUpdated.Invoke();
        }

        public void ResetBoost()
        {
            boost = 0;
            OnBoostUpdated.Invoke();
        }

        #region timerStuff
        public void TickAfterEnemiesTurn()
        {
            if (_owner.IsEnemy)
                return;

            TickAllEffects();
        }

        public void TickAfterPlayerTurn()
        {
            if (!_owner.IsEnemy)
                return;

            TickAllEffects();
        }

        private void TickAllEffects()
        {
            ResetBoost();
        }

        #endregion

        #region InitStuff
        public void Init(Unit unit)
        {
            _owner = unit;
        }

        public void UnInit() { }
        #endregion

    }
}