using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DuelistStanceAbility : BattleStanceAbility
    {
        [SerializeField] private List<StatEffectAndDeltaWhenOnlyOneEnemieNearby> startEffectsAndDeltas;
        private List<StatEffect> _effects;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _effects = new List<StatEffect>();
            for (int i = 0; i < startEffectsAndDeltas.Count; i++)
            {
                var startValue = startEffectsAndDeltas[i].startValue;
                _effects.Add(new StatEffect(startValue.type, startValue.Value, startValue.timeToTheEndOfEffect, startValue.effectIsConstantly));
            }
        }

        public override void ApplyEffect(bool isActive)
        {
            base.ApplyEffect(isActive);

            if (isActive)
            {
                RecalculateValue();
                EntryPoint.Instance.OnSomeoneMoved.AddListener(RecalculateValue);

                foreach (var effect in _effects)
                {
                    _owner.Stats.AddStatEffect(effect);
                }
            }
            else
            {
                EntryPoint.Instance.OnSomeoneMoved.RemoveListener(RecalculateValue);

                foreach (var effect in _effects)
                {
                    _owner.Stats.RemoveStatEffect(effect);
                }
            }
        }

        private void RecalculateValue()
        {
            int nearbyEnemiesQuantity = _owner.CurrentCell.CellNeighbours.GetAllNeighbors().Where(cell => cell.Content is not Barrier && cell.Content != null && cell.Content.IsEnemy).Count();
            int onlyOneEnemieNearbyModificator = nearbyEnemiesQuantity == 1 ? 1 : 0;

            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Value = startEffectsAndDeltas[i].startValue.Value + onlyOneEnemieNearbyModificator * startEffectsAndDeltas[i].deltaForOneEnemyNearby;
            }
        }
    }

    [Serializable]
    public class StatEffectAndDeltaWhenOnlyOneEnemieNearby
    {
        public int deltaForOneEnemyNearby;
        public StatEffect startValue;
    }
}