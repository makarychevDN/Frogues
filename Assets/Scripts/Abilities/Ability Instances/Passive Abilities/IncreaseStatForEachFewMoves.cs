using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatForEachFewMoves : PassiveAbility, IRoundTickable, IAbleToHaveCount
    {
        [SerializeField] private int stepsRequredToIncreaseStat;
        [SerializeField] private int additionalStrenght = 1;
        private int counter;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEnd.AddListener(TryToIncreaseStats);
        }

        public override void UnInit()
        {
            _owner.Movable.OnMovementEnd.RemoveListener(TryToIncreaseStats);
            base.UnInit();
        }

        public void TickAfterEnemiesTurn()
        {
            if (_owner == null || _owner.IsEnemy)
            {
                ResetEffects();
            }
        }

        public void TickAfterPlayerTurn()
        {
            if (_owner == null || !_owner.IsEnemy)
            {
                ResetEffects();
            }
        }

        public void ResetEffects()
        {
            ResetCounter();
        }

        private void TryToIncreaseStats()
        {
            counter++;

            if(counter >= stepsRequredToIncreaseStat)
            {
                ResetCounter();
            }
        }

        private int ResetCounter() => counter = 0;

        public int GetCount() => stepsRequredToIncreaseStat;
    }
}