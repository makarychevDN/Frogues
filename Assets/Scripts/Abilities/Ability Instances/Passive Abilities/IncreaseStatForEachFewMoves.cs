using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseStatForEachFewMoves : PassiveAbility, IRoundTickable, IAbleToHaveCount, IAbleToApplyStrenghtModificator
    {
        [SerializeField] private StatEffect statEffect;
        [SerializeField] private int stepsRequredToIncreaseStat;
        [SerializeField] private int additionalStrenght = 1;
        private int counter;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEnd.AddListener(TryToIncreaseStats);
            _owner.Stats.AddStatEffect(statEffect);
        }

        public override void UnInit()
        {
            _owner.Movable.OnMovementEnd.RemoveListener(TryToIncreaseStats);
            _owner.Stats.RemoveStatEffect(statEffect);
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
            statEffect.Value = 0;
        }

        private void TryToIncreaseStats()
        {
            counter++;

            if(counter >= stepsRequredToIncreaseStat)
            {
                statEffect.Value += additionalStrenght;
                ResetCounter();
            }
        }

        private int ResetCounter() => counter = 0;

        public int GetCount() => stepsRequredToIncreaseStat;

        public int GetStrenghtModificatorValue() => additionalStrenght;

        public int GetDeltaOfStrenghtValueForEachTurn() => 0;

        public int GetTimeToEndOfStrenghtEffect() => 1;

        public bool GetStrenghtEffectIsConstantly() => true;
    }
}