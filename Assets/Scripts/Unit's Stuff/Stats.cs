using System;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Stats : MonoBehaviour, IAbleToCalculateHashFunctionOfPrevisualisation, IRoundTickable
    {
        public UnityEvent OnSomethingUpdated;
        private Unit _owner;

        public int CalculateHashFunctionOfPrevisualisation() => 1;

        public void RemoveAllNonConstantlyEffects()
        {

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