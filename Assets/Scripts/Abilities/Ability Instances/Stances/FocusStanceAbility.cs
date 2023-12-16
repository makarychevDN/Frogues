using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FocusStanceAbility : BattleStanceAbility
    {
        [SerializeField] private List<StatEffect> effects;
        private Dictionary<StatEffect, int> _effectsStartValues = new Dictionary<StatEffect, int>();


        public override void Init(Unit unit)
        {
            base.Init(unit);
            
            foreach (var effect in effects)
            {
                _effectsStartValues[effect] = effect.Value;
            }
        }

        private void ResetValues()
        {
            foreach (var effect in effects)
            {
                effect.Value = _effectsStartValues[effect];
            }
        }

        public override void ApplyEffect(bool isActive)
        {
            base.ApplyEffect(isActive);

            if (isActive)
            {
                ResetValues();
                effects.ForEach(effect => _owner.Stats.AddStatEffect(effect));
            }
            else
            {
                effects.ForEach(effect => _owner.Stats.RemoveStatEffect(effect));
            }
        }

        public override void TickAfterPlayerTurn()
        {
            base.TickAfterPlayerTurn();

            if (!stanceActiveNow)
                return;

            if (_owner.IsEnemy)
                return;
        }

        public override void TickAfterEnemiesTurn()
        {
            base.TickAfterEnemiesTurn();

            if (!stanceActiveNow)
                return;

            if (!_owner.IsEnemy)
                return;
        }
    }
}