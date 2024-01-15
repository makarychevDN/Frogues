using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatForEachFewMoves : PassiveAbility, IRoundTickable
    {
        [SerializeField] private List<StatEffect> statEffects;
        [SerializeField] private int StepsRequredToIncreaseStat;
        private int counter;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEnd.AddListener(TryToIncreaseStats);
            statEffects.ForEach(statEffect => _owner.Stats.AddStatEffect(statEffect));
        }

        public override void UnInit()
        {
            _owner.Movable.OnMovementEnd.RemoveListener(TryToIncreaseStats);
            statEffects.ForEach(statEffect => _owner.Stats.RemoveStatEffect(statEffect));
            base.UnInit();
        }

        public void TickAfterEnemiesTurn()
        {
            if (_owner.IsEnemy)
            {
                ResetEffects();
            }
        }

        public void TickAfterPlayerTurn()
        {
            if (!_owner.IsEnemy)
            {
                ResetEffects();
            }
        }

        public void ResetEffects()
        {
            ResetCounter();
            statEffects.ForEach(stat => stat.Value = 0);
        }

        private void TryToIncreaseStats()
        {
            counter++;

            if(counter >= StepsRequredToIncreaseStat)
            {
                statEffects.ForEach(stat => stat.Value++);
                ResetCounter();
            }
        }

        private int ResetCounter() => counter = 0;
    }
}