using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FroguesFramework
{
    public class CornderedStanceAbility : BattleStanceAbility
    {
        [SerializeField] private List<StatEffectAndDelaForEachWallNearby> startEffectsAndDeltas;
        private List<StatEffect> _effects;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _effects = new List<StatEffect>();
            for(int i = 0; i < startEffectsAndDeltas.Count; i++)
            {
                var startValue = startEffectsAndDeltas[i].startValue;
                _effects.Add(new StatEffect(startValue.type, startValue.Value, startValue.timeToTheEndOfEffect, startValue.effectIsConstantly));                
            }            
        }

        public override void ApplyEffect(bool isActive)
        {
            base.ApplyEffect(isActive);

            if(isActive)
            {
                RecalculateValue();
                _owner.Movable.OnMovementEnd.AddListener(RecalculateValue);

                foreach(var effect in _effects)
                {
                    _owner.Stats.AddStatEffect(effect);
                }
            }
            else
            {
                _owner.Movable.OnMovementEnd.RemoveListener(RecalculateValue);

                foreach (var effect in _effects)
                {
                    _owner.Stats.RemoveStatEffect(effect);
                }
            }
        }

        private void RecalculateValue()
        {
            int barriersQuantity = _owner.CurrentCell.CellNeighbours.GetAllNeighbors().Where(cell => cell.Content is Barrier).Count();

            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Value = startEffectsAndDeltas[i].startValue.Value + barriersQuantity * startEffectsAndDeltas[i].deltaForEachWallNearby;
            }
        }
    }

    [Serializable]
    public class StatEffectAndDelaForEachWallNearby
    {
        public int deltaForEachWallNearby;
        public StatEffect startValue;
    }
}