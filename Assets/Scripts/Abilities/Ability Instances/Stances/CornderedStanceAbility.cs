using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FroguesFramework
{
    public class CornderedStanceAbility : BattleStanceAbility, IAbleToApplyStrenghtModificator, IAbleToApplyDexterityModificator, IAbleToHaveDelta, IAbleToHaveAlternativeDelta
    {
        [SerializeField] private bool addBonusForEachWallNearby;
        [SerializeField] private List<StatEffectAndDelaForEachWallNearby> startEffectsAndDeltas;
        private List<StatEffect> _effects;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _effects = new List<StatEffect>();
            for(int i = 0; i < startEffectsAndDeltas.Count; i++)
            {
                var startValue = startEffectsAndDeltas[i].startValue;
                _effects.Add(new StatEffect(startValue.type, startValue.Value, startValue.timeToTheEndOfEffect, startValue.deltaValueForEachTurn, startValue.effectIsConstantly));                
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
            int multiplier = addBonusForEachWallNearby ? barriersQuantity : Mathf.Clamp(barriersQuantity, 0, 1);

            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Value = startEffectsAndDeltas[i].startValue.Value + multiplier * startEffectsAndDeltas[i].additionalValueForWallsNearby;
            }
        }

        public int GetStrenghtModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.strenght).startValue.Value;

        public int GetDeltaOfStrenghtValueForEachTurn() => 0;

        public int GetTimeToEndOfStrenghtEffect() => int.MaxValue;

        public bool GetStrenghtEffectIsConstantly() => true;

        public int GetDexterityModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.dexterity).startValue.Value;

        public int GetDeltaOfDexterityValueForEachTurn() => 0;

        public int GetTimeToEndOfDexterityEffect() => int.MaxValue;

        public bool GetDexterityEffectIsConstantly() => true;

        public int GetDeltaValue()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.strenght).additionalValueForWallsNearby;
            return delta - Mathf.Abs(GetStrenghtModificatorValue());
        }

        public int GetStepValue() => 1;

        public int GetAlternativeDeltaValue()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.dexterity).additionalValueForWallsNearby;
            return delta - Mathf.Abs(GetDexterityModificatorValue());
        }

        public int GetAlternativeStepValue() => 1;
    }

    [Serializable]
    public class StatEffectAndDelaForEachWallNearby
    {
        public int additionalValueForWallsNearby;
        public StatEffect startValue;
    }
}